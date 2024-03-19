import * as THREE from 'three';
import { STLLoader } from 'three/addons/loaders/STLLoader.js';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';

function updateRendererSize(renderer, camera, cadId) {
    const parentContainer = document.getElementById(`model-${cadId}`);
    const width = parentContainer.clientWidth;
    const height = parentContainer.clientHeight;

    renderer.setSize(width, height);
    camera.aspect = width / height;
    camera.updateProjectionMatrix();
}

function loadModel({ id, name, texture, x, y, z, axis, speed, fov }) {
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
    updateRendererSize(renderer, camera, id);

    // Lights
    const directionalLight = new THREE.DirectionalLight(0xffffff, 1);
    directionalLight.position.set(1, 1, 1).normalize();
    scene.add(directionalLight);

    const ambientLight = new THREE.AmbientLight(0xffffff, 0.5);
    scene.add(ambientLight);

    // Loading
    const loader = new STLLoader();
    loader.load(`/others/cads/${name}${id}.stl`, function (stl) {
        const material = new THREE.MeshStandardMaterial();
        material.roughness = 0.5;
        material.metalness = 0.5;
        material.emissiveIntensity = 1;

        document.getElementById('submit').addEventListener('click', function () {
            const color = document.getElementById('pickColor').value;
            console.log(`Color - ${color}`);
            const hexInt = parseInt(color.replace('#', ''), 16);
            console.log(`Hex - ${hexInt}`);
            const r = ((hexInt >> 16) & 255) / 255;
            console.log(`R - ${r}`);
            const g = ((hexInt >> 8) & 255) / 255;
            console.log(`G - ${g}`);
            const b = (hexInt & 255) / 255
            console.log(`B - ${b}`);
            material.color.setRGB(r, g, b);
        });

        //const textureLoader = new THREE.TextureLoader();
        //const loadedTexture = textureLoader.load(texture);
        //loadedTexture.anisotropy = renderer.capabilities.getMaxAnisotropy();
        //material.map = loadedTexture;

        //loadedTexture.wrapS = THREE.RepeatWrapping;
        //loadedTexture.wrapT = THREE.RepeatWrapping;
        //loadedTexture.repeat.set(2, 2);

        const mesh = new THREE.Mesh(stl, material);
        scene.add(mesh);
        stl.center();
    }, undefined, function (error) {

        console.error(error);

    });

    // Controls
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

    // Animation
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
        const parentContainer = document.getElementById(`model-${id}`);
        const width = parentContainer.clientWidth;
        const height = parentContainer.clientHeight;
        updateRendererSize(renderer, camera, id);
        camera.aspect = width / height;
        camera.updateProjectionMatrix();
    });
}

export { loadModel };