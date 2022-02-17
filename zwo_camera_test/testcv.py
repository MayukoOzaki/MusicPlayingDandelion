import zwoasi as asi
import cv2
import numpy as np

# asi.init('/path/to/ASI_linux_mac_SDK_V1.17/lib/x64/libASICamera2.so')
asi.init('.\\ASICamera2.dll')

num_cameras = asi.get_num_cameras()
if num_cameras == 0:
    raise ValueError('No cameras found')

camera_id = 0  # use first camera from list
cameras_found = asi.list_cameras()
print(cameras_found)
camera = asi.Camera(camera_id)
camera_info = camera.get_camera_property()
print(camera_info)

# Get all of the camera controls
print('')
print('Camera controls:')
controls = camera.get_controls()
for cn in sorted(controls.keys()):
    print('    %s:' % cn)
    for k in sorted(controls[cn].keys()):
        print('        %s: %s' % (k, repr(controls[cn][k])))

# Use minimum USB bandwidth permitted
camera.set_control_value(asi.ASI_BANDWIDTHOVERLOAD, camera.get_controls()['BandWidth']['MinValue'])

# Set some sensible defaults. They will need adjusting depending upon
# the sensitivity, lens and lighting conditions used.
camera.disable_dark_subtract()

camera.set_control_value(asi.ASI_GAIN, 150)
camera.set_control_value(asi.ASI_EXPOSURE, 10000) # microseconds
camera.set_control_value(asi.ASI_WB_B, 99)
camera.set_control_value(asi.ASI_WB_R, 75)
camera.set_control_value(asi.ASI_GAMMA, 50)
camera.set_control_value(asi.ASI_BRIGHTNESS, 50)
camera.set_control_value(asi.ASI_FLIP, 0)

print('Enabling video mode')
try:
    # Force any single exposure to be halted
    camera.stop_video_capture()
    camera.stop_exposure()
except (KeyboardInterrupt, SystemExit):
    raise

camera.start_video_capture()

# Set the timeout, units are ms
timeout = (camera.get_control_value(asi.ASI_EXPOSURE)[0] / 1000) * 2 + 500
camera.default_timeout = timeout

print('Capturing a single color frame')
camera.set_image_type(asi.ASI_IMG_RGB24)
while True:
    data = camera.get_video_data()

    dataB = data[0::3]
    dataG = data[1::3]
    dataR = data[2::3]

    # print(data)
    h = camera_info['MaxHeight']
    w = camera_info['MaxWidth']
    imgR = np.frombuffer(dataR, dtype='uint8').reshape((h,w))
    imgG = np.frombuffer(dataG, dtype='uint8').reshape((h,w))
    imgB = np.frombuffer(dataB, dtype='uint8').reshape((h,w))
    img = cv2.merge((imgB,imgG,imgR))

    cv2.imshow('test',img)
    if cv2.waitKey(33)==27:     # escape key
        break
