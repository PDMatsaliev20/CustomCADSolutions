import * as THREE from 'three';
import { GLTFLoader } from 'three/addons/loaders/GLTFLoader.js';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';

function loadModel({ id, name, extension, x, y, z, fov }, path = `/others/cads/${name}${id}/${name}${extension}`) {
    // Scene
    const scene = new THREE.Scene();
    scene.background = null;

    // Camera
    const camera = new THREE.PerspectiveCamera(fov, window.innerWidth / window.innerHeight, 0.001, 100);
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
    const directionalLight = new THREE.DirectionalLight(0xffffff, 1);
    directionalLight.position.set(1, 1, 1).normalize();
    scene.add(directionalLight);

    const ambientLight = new THREE.AmbientLight(0xffffff, 0.5);
    scene.add(ambientLight);

    // GLTF Loading
    const loader = new GLTFLoader();
    loader.load(path,
        function (gltf) {
            $('#X').on('input', function () {
                x = parseFloat($('#X').val());
                if (Math.round(camera.position.x) != x) {
                    camera.position.x = x;
                }
            });

            $('#Y').on('input', function () {
                y = parseFloat($('#Y').val());
                if (Math.round(camera.position.y) != y) {
                    camera.position.y = y;
                }
            });

            $('#Z').on('input', function () {
                z = parseFloat($('#Z').val());
                if (Math.round(camera.position.z) != z) {
                    camera.position.z = z;
                }
            });

            $('#X, #Y, #Z').on('input', () => $.ajax({
                url: `/Cads/UpdateCoords/${id}?x=${x}&y=${y}&z=${z}`,
                type: 'POST',
                contentType: 'application/json',
                success: function () {
                    console.log('Success');
                },
                error: function (error) {
                    console.error('Error:', error);
                }
            }));

            scene.add(gltf.scene);
        },
        (xhr) => console.log((xhr.loaded / xhr.total * 100) + '% loaded'),
        (error) => console.error(error)
    );

    // Controls
    const controls = new OrbitControls(camera, renderer.domElement);
    controls.enableDamping = true;
    controls.dampingFactor = 0.1;

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

export { loadModel };