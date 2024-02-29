import * as THREE from 'three';
import { STLLoader } from 'three/addons/loaders/STLLoader.js';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';

function loadModel(cadId, cadName, widthConstant, heightConstant, x, y, z, axis, speed) {
    // Scene
    const scene = new THREE.Scene();
    scene.background = null;

    // Camera
    const camera = new THREE.PerspectiveCamera(90, window.innerWidth / window.innerHeight, 0.1, 1000);
    camera.position.set(x, y, z);
    camera.lookAt(0, 0, 0);

    // Renderer
    const renderer = new THREE.WebGLRenderer({ alpha: true, preserveDrawingBuffer: true });
    renderer.setSize(window.innerWidth / widthConstant, window.innerHeight / heightConstant);
    renderer.setClearColor(0x000000, 0);
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
        const material = new THREE.MeshLambertMaterial();
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

    let isInteracting = false;
    let resumeTimeout;

    controls.addEventListener('change', function (event) {
        isInteracting = true;
        clearTimeout(resumeTimeout);

        resumeTimeout = setTimeout(() => {
            isInteracting = false;
        }, 1500);
    });

    function animate() {
        requestAnimationFrame(animate);
        renderer.render(scene, camera);
        controls.update();

        if (!isInteracting) {
            scene.traverse(function (object) {
                if (object instanceof THREE.Mesh) {
                    switch (axis) {
                        case 'x': object.rotation.x += speed; break;
                        case 'y': object.rotation.y += speed; break;
                        case 'z': object.rotation.z += speed; break;
                    }
                }
            });
        }
    }
    animate();

    window.addEventListener('resize', function () {
        const width = window.innerWidth / widthConstant;
        const height = window.innerHeight / heightConstant;
        renderer.setSize(width, height);
        camera.aspect = width / height;
        camera.updateProjectionMatrix();
    });
}

export { loadModel };