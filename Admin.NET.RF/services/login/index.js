import request from '@/utils/request'

// 登录
export function signIn(data) {
    return request({
        url: '/api/sysAuth/login',
        method: 'post',
        data: data
    })
}

// 退出登录
export function signOut() {
    return request({
        url: '/api/sysAuth/logout',
        method: 'post'
    })
}

// 获取登录用户信息
export function userInfo() {
    return request({
        url: '/api/sysUser/baseInfo',
        method: 'get'
    })
}