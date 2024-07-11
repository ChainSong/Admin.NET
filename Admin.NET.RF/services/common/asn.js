import request from '@/utils/request'

// 获取asn列表
export function getAsn(data) {
    return request({
        url: '/api/wmsAsn/asnPageListRF',
        method: 'post',
        data: data
    })
}

// 获取asn详情
export function getAsnDetails(data) {
    return request({
        url: '/api/wmsAsn/asnDetailInfo',
        method: 'post',
        data: data
    })
}

// 完成接收
export function completeReception(data) {
    return request({
        url: '/api/wmsAsn/completeReception',
        method: 'post',
        data: data
    })
}

// 获取质检列表
export function getQa(data) {
    return request({
        url: '/api/wmsAsn/qaPageListRF',
        method: 'post',
        data: data
    })
}

// 完成质检
export function completeQa(data) {
    return request({
        url: '/api/wmsAsn/completeQa',
        method: 'post',
        data: data
    })
}

// 获取上架列表
export function getShelf(data) {
    return request({
        url: '/api/wmsAsn/shelfPageListRF',
        method: 'post',
        data: data
    })
}

// 获取lpn详情
export function getLpnShelfDetails(data) {
    return request({
        url: '/api/wmsAsn/lpnShelfDetailInfo',
        method: 'post',
        data: data
    })
}

// 完成上架
export function completeShelf(data) {
    return request({
        url: '/api/wmsAsn/completeShelf',
        method: 'post',
        data: data
    })
}

// 获取组托列表
export function getPalletPageList(data) {
    return request({
        url: '/api/wmsAsn/palletPageListRF',
        method: 'post',
        data: data
    })
}

// 完成组托
export function completeGroupPallet(data) {
    return request({
        url: '/api/wmsAsn/completeGroupPallet',
        method: 'post',
        data: data
    })
}

