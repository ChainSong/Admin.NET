import { ref } from "vue";
import { ElMessage } from "element-plus";
import * as THREE from "three";

export function useBoxContextMenu() {
    const showMenu = ref(false);
    const menuPosition = ref({ x: 0, y: 0 });
    const currentBoxInfo = ref<any>(null);

    function handleMenuAction(action: string) {
        showMenu.value = false;
        if (action === "详情") {
            ElMessage.info("库位号: " + (currentBoxInfo.value?.code ?? "无"));
        }
        // 可扩展更多操作
    }

    function registerContextMenu(dom: HTMLElement, camera: THREE.Camera, scene: THREE.Scene) {
        dom.addEventListener("contextmenu", (event) => {
            event.preventDefault();

            const rect = dom.getBoundingClientRect();
            const mouse = new THREE.Vector2(
                ((event.clientX - rect.left) / rect.width) * 2 - 1,
                -((event.clientY - rect.top) / rect.height) * 2 + 1
            );
            const raycaster = new THREE.Raycaster();
            raycaster.setFromCamera(mouse, camera);

            const intersects = raycaster.intersectObjects(scene.children, true);
            const boxMesh = intersects.find(i => (i.object as any).isShelfBox);

            if (boxMesh) {
                showMenu.value = true;
                menuPosition.value = { x: event.clientX, y: event.clientY };
                currentBoxInfo.value = (boxMesh.object as any).boxInfo;
            } else {
                showMenu.value = false;
            }
        });

        window.addEventListener("click", () => {
            showMenu.value = false;
        });
    }

    return {
        showMenu,
        menuPosition,
        currentBoxInfo,
        handleMenuAction,
        registerContextMenu
    };
}