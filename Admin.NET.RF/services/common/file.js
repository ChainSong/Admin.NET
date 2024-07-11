import request from '@/utils/request'



// 删除图片
export function delFile(data) {
    return request({
        url: '/api/sysFile/delete',
        method: 'post',
        data: data
    })
}
