import * as THREE from 'three';
import { GLTFLoader } from 'three/addons/loaders/GLTFLoader.js';
import { OrbitControls } from 'three/addons/controls/OrbitControls.js';

function loadModel({ id, x, y, z, fov, panx, pany, panz }, path) {
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

    const directionalLight2 = new THREE.DirectionalLight(0xffffff, 1);
    directionalLight2.position.set(-1, 1, 1).normalize();
    scene.add(directionalLight2);

    const ambientLight = new THREE.AmbientLight(0xffffff, 0.5);
    scene.add(ambientLight);

    // Controls
    const controls = new OrbitControls(camera, renderer.domElement);
    controls.enableDamping = true;
    controls.dampingFactor = 0.1;
    controls.target.set(panx, pany, panz);
    controls.update();

    // GLTF Loading
    const loader = new GLTFLoader();
    loader.load(path,
        function (gltf) {

            const onCoordChange = (coord) => {
                switch (coord) {
                    case 'x':
                        x = parseInt($('#x').val());
                        camera.position.x = x;
                        break;
                    case 'y':
                        y = parseInt($('#y').val());
                        camera.position.y = y;
                        break;
                    case 'z':
                        z = parseInt($('#z').val());
                        camera.position.z = z;
                        break;
                }
            };
            $('#x').on('input', () => onCoordChange('x'));
            $('#y').on('input', () => onCoordChange('y'));
            $('#z').on('input', () => onCoordChange('z'));

            const onPanChange = (pan) => {
                let diff;
                switch (pan) {
                    case 'panx':
                        diff = parseInt($('#panx').val()) - panx;
                        panx += diff;
                        controls.target.x = panx;
                        $('#x').val(x + diff);
                        onCoordChange('x');
                        break;
                    case 'pany':
                        diff = parseInt($('#pany').val()) - pany;
                        pany += diff;
                        controls.target.y = pany;
                        $('#y').val(y + diff);
                        onCoordChange('y');
                        break;
                    case 'panz':
                        diff = parseInt($('#panz').val()) - panz;
                        panz += diff;
                        controls.target.z = panz;
                        $('#z').val(z + diff);
                        onCoordChange('z');
                        break;
                }
            };
            $('#panx').on('input', () => onPanChange('panx'));
            $('#pany').on('input', () => onPanChange('pany'));
            $('#panz').on('input', () => onPanChange('panz'));

            $('#x, #y, #z, #panx, #pany, #panz').on('input', () => $.ajax({
                url: `/Cads/UpdateCoords/${id}?x=${x}&y=${y}&z=${z}&panx=${panx}&pany=${pany}&panz=${panz}`,
                type: 'POST',
                contentType: 'application/json',
                success: () => { },
                error: (err) => { }
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