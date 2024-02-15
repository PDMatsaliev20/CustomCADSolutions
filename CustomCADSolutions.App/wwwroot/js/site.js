import * as THREE from 'three';
import { STLLoader } from 'three/addons/loaders/STLLoader.js';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';

function loadModel(cadId, cadName, constant) {

    // Scene
    const scene = new THREE.Scene();
    scene.background = null;

    // Camera
    const camera = new THREE.PerspectiveCamera(90, window.innerWidth / window.innerHeight, 0.1, 1000);
    camera.position.set(20, 0, 20);
    camera.lookAt(10, 0, 0);

    // Renderer
    const renderer = new THREE.WebGLRenderer();
    renderer.setSize(window.innerWidth / constant, window.innerHeight / constant);
    document.getElementById(`model-${cadId}`).appendChild(renderer.domElement);

    // Lights
    const directionalLight = new THREE.DirectionalLight(0xffffff, 1);
    directionalLight.position.set(0, 1, 0);
    scene.add(directionalLight);

    const ambientLight = new THREE.AmbientLight(0xffffff, 0.5);
    scene.add(ambientLight);

    // STL Loader
    const loader = new STLLoader();
    loader.load(`/others/cads/${cadName}${cadId}.stl`, function (stl) {
        const material = new THREE.MeshPhongMaterial({ color: 'F00', shininess: 100 });
        const mesh = new THREE.Mesh(stl, material);
        scene.add(mesh);
        stl.center();
    }, undefined, function (error) {

        console.error(error);

    });

    // Animation
    const controls = new OrbitControls(camera, renderer.domElement);
    controls.enableDamping = true;
    controls.dampingFactor = 0.25;
    function animate() {
        requestAnimationFrame(animate);
        renderer.render(scene, camera);
        controls.update();
    }
    animate();

    window.addEventListener('resize', function () {
        const width = window.innerWidth / constant;
        const height = window.innerHeight / constant;
        renderer.setSize(width, height);
        camera.aspect = width / height;
        camera.updateProjectionMatrix();
    });
}