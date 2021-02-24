install Oculus Integration package, update OVRPlugin

Project Settings
- Player 
  - Other/Configuration
    - Active Input Handling: Both
  - Other/Rendering
    - Color Space: Linear
    -? Multithreadeded Rendering: on
    -? Low Overhead Mode: on
- Quality
  - Pixel Light Count: 1
  - Texture Quality: Full Res
  - Anisotropic Textures: Per Texture
  - Anti Aliasing: 4x
  - Soft Particles: off
  - Realtime Reflection Probes: on
  - Billboards Face Camera: on
- XR Plug-in Management
  - Install, check Oculus

from Oculus Integration package, import:
- SampleFramework/Core/CustomHands
- VR

add to scene:
- OVRCameraRig
- CustomHandLeft|Right, under OVRCameraRig/TrackingSpace/Left|RightHandAnchor