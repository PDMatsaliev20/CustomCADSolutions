import { useState, useEffect, useRef } from 'react';
import * as THREE from 'three';
import { GLTFLoader } from 'three/addons/loaders/GLTFLoader.js';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';
import { GetHomeCad } from '@/requests/public/home';
import Spinner from '@/components/spinner';

function ThreeJS({ cad, isHomeCad }) {
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
            camera.position.set(model.camCoordinates.x, model.camCoordinates.y, model.camCoordinates.z);

            const { x, y, z } = model.camCoordinates;
            if (!x && !y && !z) {
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
                const { x: xCam, y: yCam, z: zCam } = camera.position;
                const coords = { x: xCam, y: yCam, z: zCam };

                const { x: xPan, y: yPan, z: zPan } = controls.target;
                const panCoords = { x: xPan, y: yPan, z: zPan };

                window.dispatchEvent(new CustomEvent('SavePosition', { detail: { coords, panCoords, }}));
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

            const ambientLight = new THREE.AmbientLight(0xffffff, 0.5);
            scene.add(ambientLight);

            function addDirectionalLight(intensity, coords) {
                const light = new THREE.DirectionalLight(0xffffff, intensity);
                light.position.set(coords.x, coords.y, coords.z);
                scene.add(light);
            }
            addDirectionalLight(1, { x: 5, y: 10, z: 5 });
            addDirectionalLight(0.5, { x: -5, y: 5, z: 5 });
            addDirectionalLight(0.5, { x: 5, y: -5, z: 5 });
            addDirectionalLight(0.3, { x: 0, y: 5, z: -5 });
            addDirectionalLight(0.3, { x: 0, y: -5, z: 0 });
            addDirectionalLight(0.3, { x: -5, y: 0, z: 0 });
            addDirectionalLight(0.3, { x: 5, y: 0, z: 0 });

            const controls = new OrbitControls(camera, renderer.domElement);
            controls.enableDamping = true;
            controls.dampingFactor = 0.1;
            controls.target.set(model.panCoordinates.x, model.panCoordinates.y, model.panCoordinates.z);
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
            <Spinner />
        </>
        : <div ref={mountRef} className="w-full h-full" />;

    async function fetchHomeCad() {
        try {
            const { data } = await GetHomeCad();
        setModel(data);

        } catch (error) {
            console.error('Error fetching CAD:', error);
        }
    }
}

export default ThreeJS;