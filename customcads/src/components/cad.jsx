import * as THREE from 'three'
import { GLTFLoader } from 'three/addons/loaders/GLTFLoader.js'
import { OrbitControls } from 'three/addons/controls/OrbitControls.js'
import axios from 'axios'
import { useState, useEffect, useRef } from 'react'

function Cad({ cad, isHomeCad }) {
    const mountRef = useRef(null);
    const isTouchedRef = useRef(false);
    const workerRef = useRef(null);
    const [model, setModel] = useState({});
    const [scene, setScene] = useState(null);

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
        if (scene) {
            workerRef.current = new Worker(new URL('cad-worker.js', import.meta.url));

            workerRef.current.onmessage = (e) => {
                const { error, arrayBuffer } = e.data;

                if (error) {
                    console.error(error);
                } else if (arrayBuffer) {
                    const url = URL.createObjectURL(new Blob(new Uint8Array([arrayBuffer]), { type: 'glb' }));

                    new GLTFLoader().parse(arrayBuffer, '', (glb) => {
                        scene.add(glb.scene);
                        URL.revokeObjectURL(url);
                    }, (error) => console.error(error));
                }
            };

            const path = model.cadPath;
            switch (path.split('.').pop().toLowerCase()) {
                case 'gltf':
                    new GLTFLoader().load(path, (gltf) => {
                        scene.add(gltf.scene);
                    }, undefined, (e) => {
                        console.error(e);
                    }
                    );
                    break;
                default: workerRef.current.postMessage({ url: path }); break;
            }
        }
    }, [scene]);

    useEffect(() => {
        if (model.cadPath) {
            const scene = new THREE.Scene();
            setScene(scene);

            const camera = new THREE.PerspectiveCamera(model.fov, window.innerWidth / window.innerHeight, 0.001, 1000);
            camera.position.set(model.coords[0], model.coords[1], model.coords[2]);
            if (model.coords.every(c => c === 0)) {
                camera.position.set(0, 0, 5);
            }

            const mount = mountRef.current;
            if (mount.children.length > 0) {
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
    }, [model.id, model.cadPath]);

    return <div ref={mountRef} className="w-full h-full" />;

    async function fetchHomeCad() {
        try {
            await axios.get('https://localhost:7127/API/Home/Cad')
                .then(response => setModel(response.data))
                .catch(error => console.error(error));

        } catch (error) {
            console.error('Error fetching CAD:', error);
        }
    }
}

export default Cad;