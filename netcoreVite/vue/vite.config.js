import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [vue()],
  // 开发服务器配置
  server: {
    port: 3000, // Vue开发服务器端口（与.NET不冲突）
    watch: {
        // 监听Vue源码变化，自动编译
        disableGlobbing: false
    }
  },
  build:{
      outDir: '../wwwroot/js', // 输出到 wwwroot/js
      rollupOptions: {
          output: {
              entryFileNames: 'app.js', // 编译后的文件名
              assetFileNames: 'app.css' // 编译后的CSS文件名
          }
      }
  }
})
