import * as THREE from 'three';
import { GLTFLoader } from 'three/addons/loaders/GLTFLoader.js';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';

function loadModel({ id, x, y, z, fov }, path = `/Home/DownloadCad/${id}`) {
    // Scene
    const scene = new THREE.Scene();
    scene.background = null;

    // Camera
    const camera = new THREE.PerspectiveCamera(fov, window.innerWidth / window.innerHeight, 0.0001, 1000000);
    camera.position.set(x, y, z);
    camera.lookAt(0, 0, 0);

    // Renderer
    const parentContainer = document.getElementById(`model-${id}`);
    if (!parentContainer) {
        console.log(`Parent container for model-${id} not found.`);
        return;
    }

    const renderer = new THREE.WebGLRenderer({ alpha: true, preserveDrawingBuffer: true, antialias: true });
    renderer.setClearColor(0x000000, 0);
    parentContainer.appendChild(renderer.domElement);

    function updateRendererSize(renderer, camera, cadId) {
        const parentContainer = document.getElementById(`model-${cadId}`);
        const width = parentContainer.clientWidth;
        const height = parentContainer.clientHeight;

        renderer.setSize(width, height);
        camera.aspect = width / height;
        camera.updateProjectionMatrix();
    }
    updateRendererSize(renderer, camera, id);

    // Lights
    const directionalLight = new THREE.DirectionalLight(0xffffff, 1);
    directionalLight.position.set(1, 1, 1).normalize();
    scene.add(directionalLight);

    const ambientLight = new THREE.AmbientLight(0xffffff, 0.5);
    scene.add(ambientLight);

    // GLTF Loading
    const loader = new GLTFLoader();
    loader.load(path, (gltf) =>  scene.add(gltf.scene), undefined, (error) => console.error(error));

    // Controls
    const controls = new OrbitControls(camera, renderer.domElement);
    controls.enableDamping = true;
    controls.dampingFactor = 0.8;

    // Animation
    function animate() {
        requestAnimationFrame(animate);
        renderer.render(scene, camera);
        controls.update();
    }
    animate();

    window.addEventListener('resize', function () {
        const parentContainer = document.getElementById(`model-${id}`);
        const width = parentContainer.clientWidth;
        const height = parentContainer.clientHeight;
        updateRendererSize(renderer, camera, id);
        camera.aspect = width / height;
        camera.updateProjectionMatrix();
    });
}

export { loadModel };