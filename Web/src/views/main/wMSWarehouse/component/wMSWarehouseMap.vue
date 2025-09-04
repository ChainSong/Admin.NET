<template>
    <div class="wMSWarehouseMap-container">
        <el-dialog v-model="isShowDialog" :title="props.title" :width="1300" draggable="">
            <el-container style="height: 600px;">
                <div ref="threeContainer" style="width: 100%; height: 100%;"></div>
            </el-container>
            <template #footer>
                <span class="dialog-footer">
                    <el-button @click="cancel" size="default">取 消</el-button>
                    <el-button type="primary" @click="cancel" size="default">确 定</el-button>
                </span>
            </template>
        </el-dialog>

        <div
            v-if="showMenu"
            :style="{
                position: 'fixed',
                left: menuPosition.x + 'px',
                top: menuPosition.y + 'px',
                background: '#fff',
                border: '1px solid #ccc',
                zIndex: 9999,
                minWidth: '120px',
                boxShadow: '0 2px 8px rgba(0,0,0,0.15)'
            }"
        >
            <div style="padding: 8px; cursor: pointer;" @click="handleMenuAction('详情')">查看详情</div>
            <div style="padding: 8px; cursor: pointer;" @click="handleMenuAction('操作')">其他操作</div>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { ref, watch } from "vue";
import * as THREE from "three";
import { OrbitControls } from "three/examples/jsm/controls/OrbitControls";
import { DragControls } from "three/examples/jsm/controls/DragControls";
import { getShelf } from "/@/api/main/wMSWarehouse";

import { ElMessage } from "element-plus";
import type { FormRules } from "element-plus";
import { useBoxContextMenu } from "./useBoxContextMenu";

const {
    showMenu,
    menuPosition,
    currentBoxInfo,
    handleMenuAction,
    registerContextMenu
} = useBoxContextMenu();

const state = ref({})

let headerRuleRef = ref<any>({});
let headerRule = ref({});
let detailRuleRef = ref<any>({});
let detailRule = ref({});

var props = defineProps({
    title: {
        type: String,
        default: "",
    },
});
const emit = defineEmits(["reloadTable"]);
const isShowDialog = ref(false);

const threeContainer = ref<HTMLDivElement | null>(null);
let renderer: THREE.WebGLRenderer | null = null;
let controls: OrbitControls | null = null;

// 仓库参数
const WAREHOUSE_SIZE = 300;
const SHELF_GROUPS = 4;
const SHELF_ROWS_PER_GROUP = 4;
const SHELF_WIDTH = 4;
const SHELF_LENGTH = 10;
const SHELF_HEIGHT = 8;
const SHELF_GAP_X = 3;
const SHELF_GAP_Z = 4;
const SLOT_COUNT = 4;
const SLOT_SIZE = 0.7;
const BOX_HEIGHT = 1.2;
// 假设有一个热力图数据 heatMapData[shelfIdx][layer][slotX][slotZ]，值为具体数量（如0、20、80、120等）
// 这里演示用随机数，实际可用你的业务数据替换
function getHeatColorByCount(count: number) {
    if (count === 0) return "#fff";         // 白色
    if (count < 50) return "#4caf50";        // 绿色
    if (count <= 100) return "#ffeb3b";      // 黄色
    return "#f44336";                        // 红色
}

/**
 * 生成可整体拖动、可设置方向、每个箱子带库位标记的货架组（所有库位信息集中在 shelfPositions 配置中）
 * @param scene THREE.Scene
 * @param dragObjects 拖拽对象数组
 */
function createShelfGroup(
    scene: THREE.Scene,
    dragObjects: THREE.Object3D[]
) {
    const LEG_RADIUS = 0.08;
    const LEG_HEIGHT = SHELF_HEIGHT + 0.2;
    const BOARD_THICKNESS = 0.18;

    // 货架位置信息和所有库位信息统一配置
    // 每个 shelf 配置：位置、方向、层数、每层库位布局、每个库位的库存量和编码
    let shelfPositions = ref([
        {
            x: -15, z: -15, direction: 0,
            layers: [
                {
                    slotX: 1, slotZ: 2,
                    boxes: [
                        // 每个库位对象包含编码和库存量
                        [
                            { code: "A01-01", count: 10 },
                        ],
                        [
                            { code: "A01-03", count: 60 }
                        ]
                    ]
                },
                {
                    slotX: 2, slotZ: 2,
                    boxes: [
                        [
                            { code: "A02-01", count: 0 }
                        ],
                        [
                            { code: "A02-04", count: 110 }
                        ]
                    ]
                }
                // ...更多层
            ]
        },
        // 你可以继续添加更多货架，每个货架的层数、库位数、库存量都可以自定义
        {
            x: 0, z: -15, direction: Math.PI / 2,
            layers: [
                {
                    slotX: 3, slotZ: 1,
                    boxes: [
                        [0, 50, 120]
                    ]
                },
                {
                    slotX: 3, slotZ: 1,
                    boxes: [
                        [10, 80, 0]
                    ]
                },
                {
                    slotX: 3, slotZ: 1,
                    boxes: [
                        [0, 0, 0]
                    ]
                },
                {
                    slotX: 3, slotZ: 1,
                    boxes: [
                        [0, 0, 0]
                    ]
                }
            ]
        },
        // 你可以继续添加更多货架，每个货架的层数、库位数、库存量都可以自定义
        {
            x: 20, z: 20, direction: 0,
            layers: [
                {
                    slotX: 2, slotZ: 2, slotY: 2,
                    boxes: [
                        // 每个库位对象包含编码和库存量
                        [
                            { code: "A01-01", count: 10 },
                            { code: "A01-02", count: 0 }
                        ],
                        [
                            { code: "A01-03", count: 60 }
                        ]
                    ]
                }, {
                    slotX: 2, slotZ: 2, slotY: 2,
                    boxes: [
                        // 每个库位对象包含编码和库存量
                        [
                            { code: "S01-01", count: 10 },
                            { code: "S01-02", count: 0 }
                        ],
                        [
                            { code: "S01-03", count: 60 },
                            { code: "S01-04", count: 160 },
                        ]
                    ]
                }
            ]
        }
        // ...更多货架
    ]);
    let result = getShelf("").then((result) => {
        console.log("getShelf result");
        // console.log(result);
        // console.log(result.data);
        shelfPositions.value = result.data.result;
        shelfPositions.value.forEach((pos, shelfIdx) => {
            const shelfGroup = new THREE.Group();

            // 立柱
            const legGeometry = new THREE.CylinderGeometry(LEG_RADIUS, LEG_RADIUS, LEG_HEIGHT, 12);
            const legMaterial = new THREE.MeshLambertMaterial({ color: 0x444444 });
            [
                [LEG_RADIUS, LEG_RADIUS],
                [SHELF_WIDTH - LEG_RADIUS, LEG_RADIUS],
                [LEG_RADIUS, SHELF_LENGTH - LEG_RADIUS],
                [SHELF_WIDTH - LEG_RADIUS, SHELF_LENGTH - LEG_RADIUS]
            ].forEach(([dx, dz]) => {
                const leg = new THREE.Mesh(legGeometry, legMaterial);
                leg.position.set(dx, LEG_HEIGHT / 2, dz);
                shelfGroup.add(leg);
            });

            // 层板和箱子
            pos.layers.forEach((layerInfo, layer) => {
                const { slotX, slotZ, boxes } = layerInfo;
                const boardGeometry = new THREE.BoxGeometry(SHELF_WIDTH - LEG_RADIUS * 2, BOARD_THICKNESS, SHELF_LENGTH - LEG_RADIUS * 2);
                const boardMaterial = new THREE.MeshLambertMaterial({ color: 0xcfcfcf });
                const boardY = (layer + 1) * (SHELF_HEIGHT / (pos.layers.length + 1));
                const board = new THREE.Mesh(boardGeometry, boardMaterial);
                board.position.set(SHELF_WIDTH / 2, boardY, SHELF_LENGTH / 2);
                shelfGroup.add(board);

                // 横纵向均匀排布箱子
                const boxSize = { x: 4, y: 1.2, z: 4 };
                const slotSpaceX = (SHELF_WIDTH - LEG_RADIUS * 2 - boxSize.x) / (slotX - 1 || 1);
                const slotSpaceZ = (SHELF_LENGTH - LEG_RADIUS * 2 - boxSize.z) / (slotZ - 1 || 1);

                for (let xIdx = 0; xIdx < slotX; xIdx++) {
                    for (let zIdx = 0; zIdx < slotZ; zIdx++) {
                        console.log("code");
                      
                        // 取出库位对象
                        const boxInfo = (boxes[zIdx] && boxes[zIdx][xIdx]) ? boxes[zIdx][xIdx] : { code: "", count: 0 };
                        const count = boxInfo.count || 0;
                        const code = boxInfo.code || "";

                        console.log("boxInfo");
                        console.log(boxInfo);
                        console.log(boxes[zIdx]);
                        console.log(boxes[zIdx][xIdx]);
                        const boxGeometry = new THREE.BoxGeometry(boxSize.x, boxSize.y, boxSize.z);

                        // 只为左右两个面（1:右面, 3:左面）生成带库位号和热力色的材质，其余用普通材质
                        const labelMaterials: THREE.Material[] = [];
                        for (let i = 0; i < 6; i++) {
                            if (i === 1 || i === 0) {
                                // 右面和左面
                                const canvas = document.createElement('canvas');
                                canvas.width = 128;
                                canvas.height = 32;
                                const ctx = canvas.getContext('2d')!;
                                ctx.fillStyle = getHeatColorByCount(count); // 用分段色
                                ctx.fillRect(0, 0, 128, 32);
                                ctx.font = "bold 18px Arial";
                                ctx.fillStyle = "#333";
                                ctx.fillText(
                                    code,
                                    10,
                                    22
                                );
                                const texture = new THREE.CanvasTexture(canvas);
                                labelMaterials.push(new THREE.MeshBasicMaterial({ map: texture, transparent: true }));
                            } else {
                                // 其他面
                                labelMaterials.push(new THREE.MeshLambertMaterial({ color: 0xf5d398 }));
                            }
                        }

                        const box = new THREE.Mesh(boxGeometry, labelMaterials);
                        box.position.set(
                            LEG_RADIUS + boxSize.x / 2 + xIdx * slotSpaceX,
                            boardY + BOARD_THICKNESS / 2 + boxSize.y / 2,
                            LEG_RADIUS + boxSize.z / 2 + zIdx * slotSpaceZ
                        );
                        shelfGroup.add(box);
                    }
                }
            });

            // 设置货架整体位置和方向
            shelfGroup.position.set(pos.x, 0, pos.z);
            shelfGroup.rotation.y = pos.direction || 0;
            scene.add(shelfGroup);
            dragObjects.push(shelfGroup);
        });
    });
    // console.log("result");
    // console.log(result);
    // console.log(result.data);
    // shelfPositions.value = result.data.result;


}

/**
 * 生成仓库地面、区域色块、边界线、分区标识牌
 * @param scene THREE.Scene
 */
function createWarehouse(scene: THREE.Scene) {
    // 地面
    const floorGeometry = new THREE.PlaneGeometry(WAREHOUSE_SIZE, WAREHOUSE_SIZE);
    const floorMaterial = new THREE.MeshPhongMaterial({ color: 0xe0e0e0 });
    const floor = new THREE.Mesh(floorGeometry, floorMaterial);
    floor.rotation.x = -Math.PI / 2;
    scene.add(floor);

    // 区域色块
    const areaGeometry1 = new THREE.PlaneGeometry(20, 20);
    const areaMaterial1 = new THREE.MeshBasicMaterial({ color: 0x1e5b9e, transparent: true, opacity: 0.18 });
    const area1 = new THREE.Mesh(areaGeometry1, areaMaterial1);
    area1.position.set(-12, 0.01, 10);
    area1.rotation.x = -Math.PI / 2;
    scene.add(area1);

    const areaGeometry2 = new THREE.PlaneGeometry(15, 20);
    const areaMaterial2 = new THREE.MeshBasicMaterial({ color: 0x8bc34a, transparent: true, opacity: 0.15 });
    const area2 = new THREE.Mesh(areaGeometry2, areaMaterial2);
    area2.position.set(15, 0.01, 10);
    area2.rotation.x = -Math.PI / 2;
    scene.add(area2);

    // 边界线
    const borderGeometry = new THREE.EdgesGeometry(new THREE.BoxGeometry(WAREHOUSE_SIZE, 0.1, WAREHOUSE_SIZE));
    const borderMaterial = new THREE.LineBasicMaterial({ color: 0x333333 });
    const border = new THREE.LineSegments(borderGeometry, borderMaterial);
    border.position.y = 0.05;
    scene.add(border);

    // 分区标识牌
    const createLabel = (text: string, pos: [number, number, number], color = "#1e5b9e") => {
        const canvas = document.createElement('canvas');
        canvas.width = 256;
        canvas.height = 64;
        const ctx = canvas.getContext('2d')!;
        ctx.fillStyle = "#fff";
        ctx.fillRect(0, 0, 256, 64);
        ctx.font = "bold 32px Arial";
        ctx.fillStyle = color;
        ctx.fillText(text, 20, 44);
        const texture = new THREE.CanvasTexture(canvas);
        const labelGeometry = new THREE.PlaneGeometry(4, 1);
        const labelMaterial = new THREE.MeshBasicMaterial({ map: texture, transparent: true });
        const label = new THREE.Mesh(labelGeometry, labelMaterial);
        label.position.set(...pos);
        label.rotation.y = -Math.PI / 6;
        scene.add(label);
    };
    // createLabel("分拣区", [-10, 7, 10], "#1e5b9e");
    // createLabel("存储区", [10, 10, 10], "#8bc34a");
}

const renderWarehouseMap = () => {
    if (!threeContainer.value) return;

    // 清理旧的渲染器
    if (renderer) {
        renderer.dispose();
        if (threeContainer.value.firstChild) {
            threeContainer.value.removeChild(threeContainer.value.firstChild);
        }
    }

    // 场景
    const scene = new THREE.Scene();
    scene.background = new THREE.Color(0xf0f0f0);

    // 生成仓库
    createWarehouse(scene);

    // 生成货架
    const dragObjects: THREE.Object3D[] = [];
    createShelfGroup(scene, dragObjects);

    // 灯光
    const light = new THREE.DirectionalLight(0xffffff, 1.2);
    light.position.set(50, 100, 50);
    scene.add(light);
    scene.add(new THREE.AmbientLight(0xffffff, 0.7));

    // 相机
    const width = threeContainer.value.clientWidth;
    const height = threeContainer.value.clientHeight;
    const camera = new THREE.PerspectiveCamera(45, width / height, 1, 1000);
    camera.position.set(60, 60, 90);
    camera.lookAt(new THREE.Vector3(0, 10, 0));

    // 渲染器
    renderer = new THREE.WebGLRenderer({ antialias: true });
    renderer.setSize(width, height);
    threeContainer.value.appendChild(renderer.domElement);

    // OrbitControls 让地图可拖动旋转
    controls = new OrbitControls(camera, renderer.domElement);
    controls.enableDamping = true;
    controls.dampingFactor = 0.08;
    controls.screenSpacePanning = false;
    controls.minDistance = 20;
    controls.maxDistance = 200;
    controls.maxPolarAngle = Math.PI / 2.1;

    // 拖拽控制
    const dragControls = new DragControls(dragObjects, camera, renderer.domElement);
    dragControls.addEventListener('dragstart', function () {
        controls.enabled = false;
    });
    dragControls.addEventListener('dragend', function () {
        controls.enabled = true;
    });

    // 注册右键菜单事件
    registerContextMenu(renderer.domElement, camera, scene);

    // ====== 新增：鼠标缩放以鼠标点为中心 ======
    renderer.domElement.addEventListener('wheel', (event) => {
        // 只在缩放时处理
        if (!controls) return;

        // 获取鼠标在canvas上的归一化坐标
        const rect = renderer.domElement.getBoundingClientRect();
        const mouse = new THREE.Vector2(
            ((event.clientX - rect.left) / rect.width) * 2 - 1,
            -((event.clientY - rect.top) / rect.height) * 2 + 1
        );

        // 使用 Raycaster 射线投射到地面（y=0平面）
        const raycaster = new THREE.Raycaster();
        raycaster.setFromCamera(mouse, camera);

        // 地面平面
        const plane = new THREE.Plane(new THREE.Vector3(0, 1, 0), 0);
        const intersectPoint = new THREE.Vector3();
        raycaster.ray.intersectPlane(plane, intersectPoint);

        // 只有在有交点时才设置 target
        if (intersectPoint) {
            controls.target.copy(intersectPoint);
        }
        // controls.update(); // 不需要手动 update，OrbitControls 会自动处理
    }, { passive: true });
    // ====== 新增结束 ======

    // 动画渲染
    function animate() {
        requestAnimationFrame(animate);
        controls && controls.update();
        renderer && renderer.render(scene, camera);
    }
    animate();
};

watch(isShowDialog, (val) => {
    if (val) {
        setTimeout(() => {
            renderWarehouseMap();
        }, 100);
    }
});
// 打开弹窗
const openDialog = (row: any) => {
    // ruleForm.value = JSON.parse(JSON.stringify(row));
    let result = getShelf("");
    isShowDialog.value = true;
};
defineExpose({ openDialog });
</script>
