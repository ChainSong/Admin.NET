import request from '@/utils/request'

// 获取菜单树
export function getMenu() {
    return request({
        url: '/api/sysMenu/loginMenuTree',
        method: 'get'
    })
}
