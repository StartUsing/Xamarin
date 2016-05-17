# Xamarin
Xamarin.Forms中的一些小例子

# 声明
1.本文所有借鉴都有出处如有遗漏请留言指正或者Email:lichen@fissoft.com

2.本文在于实现Xamarin.Forms的一些基本实例,大神路过如有发现错误,请多加指正. 

# 示例目录

1.自定义TablePage(大家都喜欢看IOS版本的,所以这里的一个示例将是以IOS版本的为蓝图做一个IOS,Android,WindowsPhone上都能用的TablePage)

2.ListView(下拉刷新,底部自动加载)

3.Xamarin中调用各种API(照相,相册选择照片,录音,播放音频,等等.)

4.各种动画效果示例.

# 以上引用有

1. ZouJian大牛          https://github.com/chsword
2. XLabs                https://github.com/XLabs/Xamarin-Forms-Labs
3. ToastIOS             暂无
4. LiChen(It's me)      https://github.com/StartUsing

感谢以上共享的文章和帮助.（以上排名要分先后.）

# 项目引用
所有项目需要引用 Xlabs.Forms (NuGet).

IOS项目需要引用  XamarinTest.App.IOS/DependencyService/ToastIOS.dll   (原谅我不知道作者是谁.)

# Project目录

XamarinTest.APP

├──DependencyService //一些接口

│   ├──IAudio //音频

│   ├──IImageService //调整图片

│   ├──IMakeTextShow //文本提示

│   ├──ISaceAndLoad  //文件读写

├──Extensions //一些拓展方法

├──Views //页面类

