import * as THREE from 'three';
import { GLTFLoader } from 'three/addons/loaders/GLTFLoader.js';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';

function loadModel({ id, x, y, z, fov, panx, pany, panz, pan }, path) {
    // Scene
    const scene = new THREE.Scene();
    scene.background = null;

    // Camera
    const camera = new THREE.PerspectiveCamera(fov, window.innerWidth / window.innerHeight, 0.01, 10000);
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

    // Controls
    const controls = new OrbitControls(camera, renderer.domElement);
    controls.enableDamping = true;
    controls.dampingFactor = 0.1;

    function addPanning(axis, pan) {
        const panOffset = new THREE.Vector3();
        switch (axis) {
            case 'x': panOffset.copy(new THREE.Vector3(1, 0, 0)).multiplyScalar(pan); break;
            case 'y': panOffset.copy(new THREE.Vector3(0, 1, 0)).multiplyScalar(pan); break;
            case 'z': panOffset.copy(new THREE.Vector3(0, 0, 1)).multiplyScalar(pan); break;
        }

        camera.position.add(panOffset);
        controls.target.add(panOffset);
        controls.update();
    }
    addPanning('x', panx);
    addPanning('y', pany);
    addPanning('z', panz);

    function removePanning(axis, pan) {
        const panOffset = new THREE.Vector3();
        switch(axis) {
            case 'x': panOffset.copy(new THREE.Vector3(-1, 0, 0)).multiplyScalar(pan); break;
            case 'y': panOffset.copy(new THREE.Vector3(0, -1, 0)).multiplyScalar(pan); break;
            case 'z': panOffset.copy(new THREE.Vector3(0, 0, -1)).multiplyScalar(pan);break;
        }

        camera.position.add(panOffset);
        controls.target.add(panOffset);
        controls.update();
    }

    // GLTF Loading
    const loader = new GLTFLoader();
    loader.load(path,
        function (gltf) {

            $('#PanX').on('input', () => {
                removePanning('x', panx);
                panx = parseInt($('#PanX').val());
                addPanning('x', panx);
            });

            $('#PanY').on('input', () => {
                removePanning('y', pany);
                pany = parseInt($('#PanY').val());
                addPanning('y', pany);
            });

            $('#PanZ').on('input', () => {
                removePanning('z', panz);
                panz = parseInt($('#PanZ').val());
                addPanning('z', panz);
            });

            $('#X').on('input', function () {
                x = parseInt($('#X').val());
                if (Math.round(camera.position.x) != x) {
                    camera.position.x = x;
                }
            });

            $('#Y').on('input', function () {
                y = parseInt($('#Y').val());
                if (Math.round(camera.position.y) != y) {
                    camera.position.y = y;
                }
            });

            $('#Z').on('input', function () {
                z = parseInt($('#Z').val());
                if (Math.round(camera.position.z) != z) {
                    camera.position.z = z;
                }
            });

            $('#X, #Y, #Z, #PanX, #PanY, #PanZ').on('input', () => $.ajax({
                url: `/Cads/UpdateCoords/${id}?x=${x}&y=${y}&z=${z}&panx=${panx}&pany=${pany}&panz=${panz}`,
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