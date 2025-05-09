module.exports = {
    mount: {
        'public': '/',
        'src': '/dist'
    },
    plugins: [ // この行を追加
        '@snowpack/plugin-react-refresh' // この行を追加
    ]
}