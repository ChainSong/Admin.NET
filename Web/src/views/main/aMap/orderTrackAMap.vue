<template>
    <div class="aMap-container">
        <div id="map" style="width: 1000px; height: 700px;"></div>
    </div>
</template>

<script lang="ts" setup="" name="aMap">
import { ref, onMounted } from "vue";
import { ElMessageBox, ElMessage } from "element-plus";
import { auth } from '/@/utils/authFunction';
import TableColumns from "/@/entities/tableColumns";
import { number } from "echarts";
import orderStatus from "/@/entities/orderStatus";
import { useUserInfo } from '/@/stores/userInfo';
import { storeToRefs } from 'pinia';
import { Local, Session } from '/@/utils/storage';
import { downloadByData, getFileName } from '/@/utils/download';
import { classNameToArray } from "element-plus/es/utils";
import { getOrderLocation } from "/@/api/main/wMSOrder";

import AMapLoader from '@amap/amap-jsapi-loader';
import { id } from "element-plus/es/locale";
interface Props {
    value?: any;
    // idValue:any;
}

const props = withDefaults(defineProps<Props>(), {
    value: '',

});
// import { useRoute } from 'vue-router'

const state = ref({
    vm: {
        id: "",
    },
});
window._AMapSecurityConfig = {
    securityJsCode: "c954ecb06804b191f8ddf62f8bdd1155", //填写你的安全密钥
};
// 设置一个图标对象
var icon = {
    // 图标类型，现阶段只支持 image 类型
    type: 'image',
    // 图片 url
    image:
        'https://a.amap.com/jsapi_demos/static/demo-center/marker/express2.png',
    // 图片尺寸
    size: [64, 30],
    // 图片相对 position 的锚点，默认为 bottom-center
    anchor: 'center',
};
var textStyle = {
    fontSize: 12,
    fontWeight: 'normal',
    fillColor: '#22886f',
    strokeColor: '#fff',
    strokeWidth: 2,
    fold: true,
    padding: '2, 5',
};
var geocoder = ref({});
var path = ref([]);
const list = ref<any>();
var infoWindow = ref({});
const map = ref({});

// 页面加载时
onMounted(async () => {
    state.value.vm.id = props.value;
    console.log("adasd" + props.value)
    getMap();
});
// 打开弹窗
const openDialog = (row: any) => {
    state.value.vm.id = row.id;
    path.value = [];
    getMap()
};

const getMap = async () => {
    console.log("获取地图")
    console.log(state.value.vm.id)

    AMapLoader.load({
        key: '6d8b692c64f5b5fcb23dfc2a6ec0b63d',
        version: '2.0',
        plugins: ['AMap.Geolocation', 'AMap.Geocoder', 'AMap.PlaceSearch', 'AMap.Scale', 'AMap.OverView', 'AMap.ToolBar', 'AMap.MapType', 'AMap.PolyEditor', 'AMap.CircleEditor'],
        AMapUI: {
            version: '1.1',
            plugins: [],
        },
    }).then((AMap) => {
        map.value = new AMap.Map('map', {
            resizeEnable: true,
            zoom: 10,
            center: [116.397428, 39.90923], // 初始化地图中心点
        });
        geocoder = new AMap.Geocoder({
            // city 指定进行编码查询的城市，支持传入城市名、adcode 和 citycode
            city: '全国'
        })
        infoWindow = new AMap.InfoWindow({ offset: new AMap.Pixel(0, -30) });
        getLocation(this, function (self, data) {
            var polyline1 = new AMap.Polyline({
                path: path.value,
                strokeColor: "#22886f",
                strokeWeight: 6,
                strokeOpacity: 0.9,
                zIndex: 50,
                bubble: true,
            })
            map.value.add([polyline1]);

            // console.log("data")
            // console.log(data)
            var flag = true;
            data.forEach((item: any) => {
                const marker = new AMap.Marker({
                    icon: flag ? "/images/map/poi-marker-red.png" : "/images/map/poi-marker-default.png",
                    position: [item.location.lng, item.location.lat],
                    offset: new AMap.Pixel(-23, -55),
                    map: map,
                });
                flag = false;
                // console.log("item")
                // console.log(item.formattedAddress)
                marker.content = ("<div class='input-card content-window-card'><div>" + item.formattedAddress + "</div> ");
                marker.on('click', markerClick);
                marker.emit('click', { target: marker });
                map.value.add(marker);
                //构建信息窗体中显示的内容
                // var info = [];
                // info.push("<div class='input-card content-window-card'><div><img style=\"float:left;width:67px;height:16px;\" src=\" https://webapi.amap.com/images/autonavi.png \"/></div> ");
                // info.push("<div style=\"padding:7px 0px 0px 0px;\"><h4>高德软件</h4>");
                // info.push("<p class='input-item'>电话 : 010-84107000   邮编 : 100102</p>");
                // info.push("<p class='input-item'>地址 :北京市朝阳区望京阜荣街10号首开广场4层</p></div></div>");

                // var infoWindow = new AMap.InfoWindow(
                //     {
                //         position: [item.location.lng, item.location.lat],
                //     content: info.join("")  //使用默认信息窗体框样式，显示信息内容
                // });

                // infoWindow.open(map, map.getCenter());
            });
            // 缩放地图到合适的视野级别
            map.value.setFitView();
        })

    }).catch(e => {
        console.log(e);
    });

    // getOrderLocation
}
const markerClick = (e: any) => {
    // console.log("e")
    // console.log(e)
    // console.log(map)
    // console.log( e.target.getPosition())
    infoWindow.setContent(e.target.content);
    infoWindow.open(map.value, e.target.getPosition());
}
const getLocation = async (self, callBack) => {
    path.value = [];
    let results = await getOrderLocation(state.value.vm.id);
    // console.log("results")
    // console.log(results)
    if (results.data.result.code == 1) {
        // console.log(geocoder.value.getLocation("北京市朝阳区阜荣街100号"));
        // 使用geocoder做地理/逆地理编码
        geocoder.getLocation(results.data.result.data, function (status: any, result: any) {
            // console.log("status")
            // console.log(status)
            // console.log(results.data.result.data)
            // console.log(result)
            if (status === 'complete' && result.info === 'OK') {
                var flag = 0;
                result.geocodes.forEach((item: any) => {
                    if (flag < results.data.result.data.length) {
                        // console.log("item.location.lng, item.location.lat")
                        // console.log([item.location.lng, item.location.lat])
                        path.value.push([item.location.lng, item.location.lat]);
                        flag++;
                    }
                    // console.log(path)
                })
                callBack(self, result.geocodes)
                // result中对应详细地理坐标信息
            } else {
                console.log('获取位置失败', result.error);
            }
        })
    }
}
//将属性或者函数暴露给父组件
defineExpose({ openDialog });
</script>