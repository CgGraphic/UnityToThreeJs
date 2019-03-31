![alt text](/Preview.PNG)
Exported scene from Unity to ThreeJS

To export scene select Tools -> UnityToThreeJs -> Export (or use hotkey ctrl+shift+alt+p)
Export results you may see in logs.

Features:
- Exports object hierarchy (with childs)
- DirectionLight as DirectionLight
- Camera as PerspectiveCamera
- Meshes as BufferGeometry
- TODO: matrials and textures support

Attention!
1) Unity uses Right-handed coordinate system, ThreeJS(WebGL) uses Left-handed coordinate system! (to workaround it, x part in projectionMatrix in TheeJS side is flipped)
2) Unity camera looks in z direction, but Thee.js camera looks in -z direction! (to workaround it, camera transform matrix is rotated 180 degrees around local up axis)
3) Plane is not rendered because of backface culling (after flip x)
4) If you have problems with GoogleChrome security, use "C:\Program Files (x86)\Google\Chrome\Application\chrome.exe" --user-data-dir="%PATH_TO_RESULT%" --disable-web-security or start index.html from IDE (JetbrainsRider does the job)

Usefull Links
- https://threejs.org/docs/
- https://threejs.org/examples/
- https://github.com/mrdoob/three.js/wiki/JSON-Object-Scene-format-4
- http://www.opengl-tutorial.org/beginners-tutorials/tutorial-3-matrices/

For debug in UnitySceneRunner you can change active camera to debug camera (line 47-48) to easily watch scene from different side with orbit control (Use mouse)
