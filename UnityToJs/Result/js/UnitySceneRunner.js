var controls;
var mainCamera, unityCamera, debugCamera, scene, renderer;
var loadedHierarchy;
var loader;

var currentScene = null;
var isTestSceneLoaded = false;

init();
animate();

function loadScene(sceneName) {

    currentScene = sceneName;

    loader = new THREE.ObjectLoader();

    THREE.Cache.clear();
    setTimeout(function () {
        renderer.setClearColor(0xe675F57);
        if (loadedHierarchy) {
            scene.remove(loadedHierarchy);
        }
        loader.load('ExportedScene.js?v1_1', function (obj) {
            loadedHierarchy = obj;
            
            scene.add(loadedHierarchy);
            var axesHelper = new THREE.AxisHelper(100);
            scene.add(axesHelper);

            unityCamera = getCamera(loadedHierarchy);
            unityCamera.aspect = window.innerWidth / window.innerHeight;
            unityCamera.updateProjectionMatrix();

            var unityCameraHelper = new THREE.CameraHelper(unityCamera);
            scene.add(unityCameraHelper);

            debugCamera = new THREE.PerspectiveCamera( 70, window.innerWidth / window.innerHeight, 1, 1000 );
            debugCamera.position.copy(new THREE.Vector3(-unityCamera.position.x * 2,unityCamera.position.y * 2, -unityCamera.position.z * 2));
            debugCamera.quaternion.copy(unityCamera.quaternion);

            // controls is used for debug
            controls = new THREE.OrbitControls( debugCamera );
            controls.damping = 0.2;
            controls.target.copy(new THREE.Vector3(0,0,0));

            mainCamera = unityCamera;
            // mainCamera = debugCamera;
            
            // https://threejs.org/docs/#api/en/cameras/Camera (Note: A camera looks down its local, negative z-axis).
            unityCamera.updateMatrixWorld();
            mainCamera.projectionMatrix.scale(new THREE.Vector3(-1,1,1));
        });
    }, 500)
}

function getCamera(hierarchy){
    for (var i = 0; i < hierarchy.children.length; i++) {
        if (hierarchy.children[i].type == "PerspectiveCamera")
            return hierarchy.children[i];
    }
}

function init() {
    renderer = new THREE.WebGLRenderer();
    renderer.setPixelRatio(window.devicePixelRatio);
    renderer.setSize(window.innerWidth, window.innerHeight);

    document.body.appendChild(renderer.domElement);

    scene = new THREE.Scene();

    window.addEventListener('resize', onWindowResize, false);

    loadScene('SimpleScene');
}

function onWindowResize() {
    if (mainCamera == null)
        return;

    mainCamera.aspect = window.innerWidth / window.innerHeight;
    mainCamera.updateProjectionMatrix();
    mainCamera.projectionMatrix.scale(new THREE.Vector3(-1,1,1));

    renderer.setSize(window.innerWidth, window.innerHeight);
}

function animate() {
    if (currentScene !== 'test' || !isTestSceneLoaded) requestAnimationFrame(animate);
    if (mainCamera == null) return;

    renderer.render(scene, mainCamera);
    console.log(debugCamera.quaternion);
}