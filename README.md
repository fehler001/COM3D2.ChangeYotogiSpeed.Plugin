## COM3D2.ChangeYotogiSpeed.Plugin
A COM3D2 plugin to adjust yotogi(animation) speed (inspired by COM3D2.AddYotogiSlider.Plugin)  
Toggle orthographic or perspective of camera view  
Adjust field of view in perspective view (inspired by COM3D2.YotogiUtil.plugin)  

## How to use
Put dll into COM3D2\BepInEx\plugins\ (need BepInEx).  

#### Change Yotogi Speed: Mode1 (constant mode)
In Yotogi scene, press ";" to turn on or turn off the plugin.  
When turning on, press "<", ">" to adjust speed.  

#### Change Yotogi Speed: Mode2 (dynamic mode)
In Yotogi scene, press " ' "(single quote) to turn on or turn off the plugin.  
When turning on, press "<", ">" to adjust speed change range in one frame  
" ↓ ", " ↑ " to adjust max speed up to, " ← ", " → " to adjust min speed down to.  
  
Either Mode turning on will turn off other Mode.  
  
##
#### Toggle orthographic or Perspective view
'Right Alt' + 'O', 'Right Alt' + 'P' to toggle orthographic or perspective view  
Advice: toggle back to perspective view before go to another scene  
If you go to another scene with toggled in orthographic view, and you feel something weird, you can toggle back to perspective view, then go to another scene, then go back, toggle to orthographic view, this should fix that weird(maybe?)
  
#### Adjust field of view
In perspective view:  
'Right Alt'+ '[', 'Right Alt' + ']' to adjust field of view  
'Right Alt' + 'backslash' to reset field of view to 35

