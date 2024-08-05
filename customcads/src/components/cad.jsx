import * as THREE from 'three'
import { GLTFLoader } from 'three/addons/loaders/GLTFLoader.js'
import { OrbitControls } from 'three/addons/controls/OrbitControls.js'
import { useState, useEffect, useRef } from 'react'
import axios from 'axios'
import { GetCad } from '@/requests/public/home'

function Cad({ cad, isHomeCad }) {
    const mountRef = useRef(null);
    const isTouchedRef = useRef(false);
    const [model, setModel] = useState({});
    const [loader, setLoader] = useState(true);

    useEffect(() => {
        if (isHomeCad) {
            fetchHomeCad();
        }
    }, [isHomeCad]);

    useEffect(() => {
        if (cad) {
            setModel(cad);
        }
    }, []);

    useEffect(() => {
        if (model.cadPath) {
            const scene = new THREE.Scene();

            const camera = new THREE.PerspectiveCamera(model.fov, window.innerWidth / window.innerHeight, 0.001, 1000);
            camera.position.set(model.coords[0], model.coords[1], model.coords[2]);
            if (model.coords.every(c => c === 0)) {
                camera.position.set(0, 0, 5);
            }

            const mount = mountRef.current;
            if (!mount || mount.children.length > 0) {
                return;
            }

            const renderer = new THREE.WebGLRenderer({ alpha: true, antialias: true });
            mount.appendChild(renderer.domElement);

            let isInteracting = false
            let resumeTimeout;

            function cadTouched() {
                if (!isTouchedRef.current) {
                    window.dispatchEvent(new CustomEvent('PositionChanged'));
                    isTouchedRef.current = true;
                }

                isInteracting = true;
                clearTimeout(resumeTimeout);

                resumeTimeout = setTimeout(() => {
                    isInteracting = false;
                }, 1500);
            }

            function sendPosition() {
                window.dispatchEvent(new CustomEvent('SavePosition', {
                    detail: {
                        coords: [camera.position.x, camera.position.y, camera.position.z],
                        panCoords: [controls.target.x, controls.target.y, controls.target.z]
                    }
                }));
            };

            function trackChanges() {
                isTouchedRef.current = false;
            };

            function updateRendererSize() {
                const width = mount.clientWidth;
                const height = mount.clientHeight;

                renderer.setSize(width, height);
                camera.aspect = width / height;
                camera.updateProjectionMatrix();
            }
            updateRendererSize();

            const directionalLight = new THREE.DirectionalLight(isHomeCad ? 0xa5b4fc : 0xffffff, 1);
            directionalLight.position.set(1, 1, 1).normalize();
            scene.add(directionalLight);

            const directionalLight2 = new THREE.DirectionalLight(0xffffff, 1);
            directionalLight2.position.set(-1, 1, 1).normalize();
            scene.add(directionalLight2);

            const ambientLight = new THREE.AmbientLight(isHomeCad ? 0xa5b4fc : 0xffffff, 0.5);
            scene.add(ambientLight);

            const controls = new OrbitControls(camera, renderer.domElement);
            controls.enableDamping = true;
            controls.dampingFactor = 0.1;
            controls.target.set(model.panCoords[0], model.panCoords[1], model.panCoords[2]);
            controls.update();

            const loader = new GLTFLoader();
            loader.load(model.cadPath,
                (cad) => scene.add(cad.scene),
                (xhr) => xhr.loaded === xhr.total && setLoader(false),
                (e) => console.error(e)
            );

            controls.addEventListener('change', cadTouched);

            function animate() {
                requestAnimationFrame(animate);
                controls.update();
                renderer.render(scene, camera);

                if (isHomeCad && !isInteracting) {
                    scene.rotation.y += 0.01;
                }
            }
            animate();

            controls.addEventListener('change', cadTouched);
            window.addEventListener('resize', updateRendererSize);
            window.addEventListener('TrackChanges', trackChanges);
            window.addEventListener('SendPosition', sendPosition);


            return () => {
                mount.removeChild(renderer.domElement);
                controls.removeEventListener('change', cadTouched);
                window.removeEventListener('resize', updateRendererSize);
                window.removeEventListener('TrackChanges', trackChanges);
                window.removeEventListener('SendPosition', sendPosition);
            };
        }

    }, [loader, model.id, model.cadPath]);

    return !isHomeCad && loader
        ? <>
            <div ref={mountRef} className="w-full h-full hidden" />
            <div className="w-full h-full flex justify-center items-center">
                <div className="animate-spin w-12 h-12 border-4 border-indigo-500 border-b-transparent rounded-full border-box" />
            </div>
        </>
        : <div ref={mountRef} className="w-full h-full" />;

    async function fetchHomeCad() {
        try {
            const { data } = await GetCad();
            setModel(data);

        } catch (error) {
            console.error('Error fetching CAD:', error);
        }
    }
}

export default Cad;