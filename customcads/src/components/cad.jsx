import * as THREE from 'three';
import { GLTFLoader } from 'three/addons/loaders/GLTFLoader.js';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';
import axios from 'axios'
import { useState, useEffect, useRef } from 'react'

function Cad({ cad, isHomeCad }) {
    const mountRef = useRef(null);
    const [model, setModel] = useState({});

    useEffect(() => {
        if (isHomeCad) {
            populateCad(isHomeCad);
        }
    }, [isHomeCad]);

    useEffect(() => {
        if (cad) {
            setModel(cad);
        }
    }, []);

    useEffect(() => {
        if (model.path) {
            // Scene
            const scene = new THREE.Scene();
            scene.background = null;

            // Camera
            const camera = new THREE.PerspectiveCamera(model.fov, window.innerWidth / window.innerHeight, 0.001, 1000);
            camera.position.set(model.coords[0], model.coords[1], model.coords[2]);

            // Renderer
            const mount = mountRef.current;
            if (mount.children.length > 0) {
                return;
            }

            const renderer = new THREE.WebGLRenderer({ alpha: true, antialias: true });
            mount.appendChild(renderer.domElement);

            function updateRendererSize() {
                const width = mount.clientWidth;
                const height = mount.clientHeight;

                renderer.setSize(width, height);
                camera.aspect = width / height;
                camera.updateProjectionMatrix();
            }
            updateRendererSize();

            // Lights
            const directionalLight = new THREE.DirectionalLight(isHomeCad ? 0xa5b4fc : 0xffffff, 1);
            directionalLight.position.set(1, 1, 1).normalize();
            scene.add(directionalLight);

            const directionalLight2 = new THREE.DirectionalLight(0xffffff, 1);
            directionalLight2.position.set(-1, 1, 1).normalize();
            scene.add(directionalLight2);

            const ambientLight = new THREE.AmbientLight(isHomeCad ? 0xa5b4fc : 0xffffff, 0.5);
            scene.add(ambientLight);

            // Controls
            const controls = new OrbitControls(camera, renderer.domElement);
            controls.enableDamping = true;
            controls.dampingFactor = 0.1;
            controls.target.set(model.panCoords[0], model.panCoords[1], model.panCoords[2]);
            controls.update();

            const onCameraCoordChange = (name, value) => {
                switch (name.toLowerCase()) {
                    case 'x': camera.position.x = value; break;
                    case 'y': camera.position.y = value; break;
                    case 'z': camera.position.z = value; break;
                }
            };

            const onPanCoordChange = (name, value) => {
                switch (name.toLowerCase()) {
                    case 'x':
                        camera.position.x += value - controls.target.x;
                        controls.target.x = value;
                        break;

                    case 'y':
                        camera.position.y += value - controls.target.y;
                        controls.target.y = value;
                        break;

                    case 'z':
                        camera.position.z += value - controls.target.z;
                        controls.target.z = value;
                        break;

                }
            };

            const handleCoordChange = (e) => {
                const { coordType, coordName, coordValue } = e.detail;

                if (coordType === 'camera') onCameraCoordChange(coordName, coordValue);
                else if (coordType === 'pan') onPanCoordChange(coordName, coordValue);
            };

            // GLTF Loading
            const loader = new GLTFLoader();
            loader.load(model.path, (gltf) => {
                window.addEventListener('onCoordChange', handleCoordChange);
                scene.add(gltf.scene);
            },
                () => { },
                (error) => console.error(error)
            );

            let isInteracting = false
            let resumeTimeout;

            controls.addEventListener('change', function () {
                isInteracting = true;
                clearTimeout(resumeTimeout);

                resumeTimeout = setTimeout(() => {
                    isInteracting = false;
                }, 1500);
            });

            // Animation
            function animate() {
                requestAnimationFrame(animate);
                controls.update();
                renderer.render(scene, camera);

                if (isHomeCad && !isInteracting) {
                    scene.rotation.y += 0.01;
                }
            }
            animate();

            // Adapt to screen size
            window.addEventListener('resize', updateRendererSize);

            return () => {
                mount.removeChild(renderer.domElement);
                window.removeEventListener('resize', updateRendererSize);
                window.removeEventListener('onCoordChange', handleCoordChange);
            };
        }
    }, [model.path, model.coords, model.fov, model.id]);

    return <div ref={mountRef} className="w-full h-full" />;

    async function populateCad(isHomeCad) {
        try {
            if (isHomeCad) {
                await axios.get('https://localhost:7127/API/Home/Cad')
                    .then(response => setModel(response.data))
                    .catch(error => console.error(error));
            }

        } catch (error) {
            console.error('Error fetching CAD:', error);
        }
    }
}

export default Cad;