# 基于UnityC#的Runtime进行时自定义逻辑框架及含节点编辑器的游戏Demo
---
- *项目地址* : (https://github.com/megaSukura/Technology-Demonstration-Project.git)
- *演示视频* : (https://pan.baidu.com/s/1_-a-3YONJHyycZpgBvlPtw?pwd=r5gp)
## 项目简介

- 是一个基于donet的自定义逻辑框架，主要用于游戏逻辑的编写，可以在不用重新编译的情况下，实时的修改游戏逻辑，方便调试或者创造灵活的游戏玩法。
    - 框架不基于Unity的任何功能,也未使用反射,没有使用任何第三方库,完全自己实现
    - 在这个Demo中,使用unity实现了一个简单的可视化节点编辑器,可以将节点编辑器中的逻辑转换为Runtime的逻辑
    - 框架的核心是基于多状态状态机的的理念,基于 **ScheduleLine** ,**Trigger**,**Parameter**,实现的更加通用的节点逻辑框架

