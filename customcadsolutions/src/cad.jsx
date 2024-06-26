import * as THREE from 'three';
import { GLTFLoader } from 'three/addons/loaders/GLTFLoader.js';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';
import axios from 'axios'
import { useState, useEffect } from 'react'

function Cad({ cad, id, isHomeCad }) {
    const [model, setModel] = useState({});

    useEffect(() => {
        if (cad) {
            setModel(cad);
        } else {
            populateCad(id, isHomeCad);
        }
    }, [id]);


    useEffect(() => {
        if (model.path) {
            // Scene
            const scene = new THREE.Scene();
            scene.background = null;

            // Camera
            const camera = new THREE.PerspectiveCamera(model.fov, window.innerWidth / window.innerHeight, 0.001, 100);
            camera.position.set(model.coords[0], model.coords[1], model.coords[2]);

            // Renderer
            const parentContainer = document.getElementById(`model-${model.id}`);
            if (parentContainer.children.length > 0) {
                return;
            }

            const renderer = new THREE.WebGLRenderer({ alpha: true, antialias: true });
            parentContainer.appendChild(renderer.domElement);

            function updateRendererSize(renderer, camera, id) {
                const parentContainer = document.getElementById(`model-${id}`);
                const width = parentContainer.clientWidth;
                const height = parentContainer.clientHeight;

                renderer.setSize(width, height);
                camera.aspect = width / height;
                camera.updateProjectionMatrix();
            }
            updateRendererSize(renderer, camera, model.id);

            // Lights
            const directionalLight = new THREE.DirectionalLight(0xa5b4fc, 1);
            directionalLight.position.set(1, 1, 1).normalize();
            scene.add(directionalLight);

            const ambientLight = new THREE.AmbientLight(0xa5b4fc, 0.5);
            scene.add(ambientLight);

            // GLTF Loading
            const loader = new GLTFLoader();
            loader.load(model.path, (gltf) => {
                scene.add(gltf.scene);
            },
                () => { },
                (error) => console.error(error)
            );


            // Controls
            const controls = new OrbitControls(camera, renderer.domElement);
            controls.enableDamping = true;
            controls.dampingFactor = 0.1;

            const panOffset = new THREE.Vector3();
            panOffset.copy(new THREE.Vector3(1, 0, 0)).multiplyScalar(model.panCoords[0]);
            camera.position.add(panOffset);

            panOffset.copy(new THREE.Vector3(0, 1, 0)).multiplyScalar(model.panCoords[1]);
            camera.position.add(panOffset);

            panOffset.copy(new THREE.Vector3(0, 0, 1)).multiplyScalar(model.panCoords[2]);
            camera.position.add(panOffset);

            camera.position.add(panOffset);
            controls.target.add(panOffset);
            controls.update();
            
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
                renderer.render(scene, camera);
                controls.update();

                if (!isInteracting) {
                    scene.rotation.y += 0.01;
                }
            }
            animate();

            // Adapt to screen size
            window.addEventListener('resize', function () {
                const width = parentContainer.clientWidth;
                const height = parentContainer.clientHeight;
                updateRendererSize(renderer, camera, model.id);
                camera.aspect = width / height;
                camera.updateProjectionMatrix();
            });
        }

    }, [model.path, model.coords, model.fov, model.id]);

    async function populateCad(id, isHomeCad) {
        try {
            if (isHomeCad) {
                await axios.get('https://localhost:7127/API/Home/Cad')
                    .then(response => setModel(response.data))
                    .catch(error => console.error(error));
            } else {
                await axios.get(`https://localhost:7127/API/Cads/${id}`, { withCredentials: true })
                    .then(response => setModel(response.data))
                    .catch(error => console.error(error));
            }

        } catch (error) {
            console.error('Error fetching CAD:', error);
        }
    }
}

export default Cad;