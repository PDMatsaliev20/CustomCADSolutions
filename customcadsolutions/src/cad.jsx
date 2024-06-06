import * as THREE from 'three';
import { GLTFLoader } from 'three/addons/loaders/GLTFLoader.js';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';
import axios from 'axios'
import { useState, useEffect } from 'react'

function Cad({ id, isHomeCad, token }) {

    const [model, setModel] = useState({});

    useEffect(() => {
        populateCad(id, isHomeCad);
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
            const parentContainer = document.getElementById(`model-${id}`);
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
            updateRendererSize(renderer, camera, id);

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
            controls.mouseButtons = {
                LEFT: THREE.MOUSE.ROTATE,
                MIDDLE: THREE.MOUSE.DOLLY,
                RIGHT: THREE.MOUSE.PAN
            }


            // Animation
            function animate() {
                requestAnimationFrame(animate);
                controls.update();
                renderer.render(scene, camera);
            }
            animate();

            // Adapt to screen size
            window.addEventListener('resize', function () {
                const width = parentContainer.clientWidth;
                const height = parentContainer.clientHeight;
                updateRendererSize(renderer, camera, id);
                camera.aspect = width / height;
                camera.updateProjectionMatrix();
            });
        }

    }, [model.path, model.coords, model.fov, id]);

    async function populateCad(id, isHomeCad) {
        try {
            const headers = {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token
            };


            if (isHomeCad) {
                await axios.get('https://localhost:7127/API/Home/Cad')
                    .then(response => setModel(response.data))
                    .catch(error => console.error(error));
            } else {
                await axios.get(`https://localhost:7127/API/Cads/${id}`, { headers })
                    .then(response => setModel(response.data))
                    .catch(error => console.error(error));
            }

        } catch (error) {
            console.error('Error fetching CAD:', error);
        }
    }
}

export default Cad;