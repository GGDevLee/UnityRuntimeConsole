# RuntimeConsole

**联系作者：419731519（QQ）**

### RuntimeConsole介绍
#### 笔者的这个插件，主要是为了能更加方便的在手机端查看Unity日志跟Android日志，以及各种移动端信息
#### 只需要配置两个依赖，无需编写任何代码，即可使用
#### 觉得我的插件能帮助到你，麻烦帮我点个Star支持一下❤️


### 功能介绍
- Mini界面，可以实时观察手机的游戏启动时间，版本，帧率，Mono内存情况
- 监听Unity层所有日志，并且每个日志都带有实时的帧率，内存情况
- 设置界面可以查看看手机的Appliction跟SystemInfo全部属性


### 安装插件
这里有3种方法安装插件
- **Packages/manifest.json**中添加以下行:
```json
{
  "dependencies": {
	"com.leeframework.uilooplistmini":"https://e.coding.net/ggdevlee/leeframework/LoopListMini.git#1.0.1",
	"com.leeframework.console":"https://e.coding.net/ggdevlee/leeframework/RuntimeConsole.git#1.1.1"
  }
}
```
- **import [RuntimeConsole.unitypackage](https://github.com/GGDevLee/UnityRuntimeConsole/blob/main/Release/RuntimeConsole.unitypackage)**
- clone/**[download](https://codeload.github.com/GGDevLee/UnityRuntimeConsole/zip/refs/heads/main)** 这个仓库，然后移动 **Unity/Assets/RuntimeConsole文件夹**到你的Unity工程Assets目录下

### =================图片演示=================
![输入图片说明](TmpGif/screenshots.gif)
![输入图片说明](TmpGif/screenshots2.gif)

