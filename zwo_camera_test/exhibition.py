import zwoasi as asi
import cv2
import subprocess
import numpy as np
import statistics
import math
import time
import random


#from PIL import Image, ImageDraw

# 検出した頂点のうちどのくらいの割合を揺らぎ検出点に使うか
POINT_RATIO = 0.001
# 二次曲線フィッティングに使う点の数
XRANGE = 3
# 画面内の検出エリアの横方向の割合
W_RATIO = 0.5
# 画面内の検出エリアの縦方向の割合
H_RATIO = 0.8

p =2121856 #1228800  # 921600 #2073600 #312664
#1280×960
wide = 1936
#1280
high = 1096
#720
#p = 1228800

#cap = cv2.VideoCapture(0)

#cap.set(cv2.CAP_PROP_FRAME_WIDTH, wide)  # カメラ画像の横幅を1920#1280に設定#646
#cap.set(cv2.CAP_PROP_FRAME_HEIGHT, high)  # カメラ画像の縦幅を1080720に設定#484


avg = None
bef_toplist = None
bef_ss = None

# 閾値の設定
#threshold = 100



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
camera.set_control_value(asi.ASI_BANDWIDTHOVERLOAD,camera.get_controls()['BandWidth']['MinValue'])

# Set some sensible defaults. They will need adjusting depending upon
# the sensitivity, lens and lighting conditions used.
camera.disable_dark_subtract()

camera.set_control_value(asi.ASI_GAIN, 150)
camera.set_control_value(asi.ASI_EXPOSURE, 10000)  # microseconds
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
"""
while True:
    data = camera.get_video_data()

    dataB = data[0::3]
    dataG = data[1::3]
    dataR = data[2::3]

    # print(data)
    h = camera_info['MaxHeight']
    w = camera_info['MaxWidth']
    imgR = np.frombuffer(dataR, dtype='uint8').reshape((h, w))
    imgG = np.frombuffer(dataG, dtype='uint8').reshape((h, w))
    imgB = np.frombuffer(dataB, dtype='uint8').reshape((h, w))
    img = cv2.merge((imgB, imgG, imgR))

    cv2.imshow('test', img)
    if cv2.waitKey(33) == 27:     # escape key
        break
"""

def reset_standard():  # 基準画像リセット
    global basetop
    global basetop2
    global avg

    avg = gray.copy().astype("float")

    base = np.asarray(avg)
    base = base.flatten()  # 元データ

    p = len(base)
    basedata = []  # 移動平均
    basetop = []  # 頂点位置

    bef_sum = 0
    for a in range(p):
        if a == 0:
            now_sum = sum(base[0:3])
            d = now_sum/3
        elif a == 1 or a == 2:
            now_sum = bef_sum+base[a+2]
            d = now_sum/(a+3)
        elif a == p-2 or a == p-1:
            now_sum = bef_sum-base[a-3]
            if a == p-2:
                d = now_sum/4
            else:
                d = now_sum/3
        else:
            now_sum = bef_sum-base[a-3]+base[a+2]
            d = now_sum/5

        basedata.append(d)
        bef_sum = now_sum

    #print("basedata")

    count = 1
    start = 0
    before = -999999
    trend = 0  # 上がっていたら１下がっていたら０
    for e in range(p):
        now = basedata[e]
        if before == now:
            count += 1
        elif before > now:
            if trend == 1:
                px = e % wide
                py = e // wide
                if abs(px - wide//2) < wide*W_RATIO/2 and abs(py - high//2) < high*H_RATIO/2:
                    basetop.append(int((start+e-1)/2))
            trend = 0
            count = 1
            start = e
        elif before < now and before >= 100:
            trend = 1
            count = 1
            start = e

        before = now

    #print("basetop")

    ql = []
    for q in range(len(basetop)-1):
        qq = basetop[q+1]-basetop[q]
        ql.append(qq)
        #print(set(ql))
        #print(sum(ql)/len(ql))
    #print("ql")
    basetop2 = []
    for p in basetop:
        if random.random() < POINT_RATIO:
            basetop2.append(p)
    #print("basetop2")
    #print(len(basetop))




def searching_top():  # 頂点検出
    global toplist
    x = list(range(XRANGE))  # list(range(35))  # 15  XRANGE=int(t_range/2)
    toplist = []
    #print(p)
    p = len(data1)
    #print(p)
    #print("data1", len(data1))

    for top in basetop2:
        top = int(top)
        if top <= int(XRANGE/2):
            y = data1[:XRANGE]
        elif top >= (p-1)-int(XRANGE/2):
            y = data1[(p-1)-(XRANGE-1):]
            #print("x", len(x))
            #print("y", len(y))
        else:
            # int(XRANGE/2),int(XRANGE/2)+1
            y = data1[top-int(XRANGE/2):top+int(XRANGE/2)+1]
        #print("x",x)
        #print("y",y)
        z = np.polyfit(x, y, 2)
        d = (-z[1]) / (2 * z[0])

        if top <= int(XRANGE/2):  # int(XRANGE/2)
            toplist.append(d)
        elif top >= (p-1)-int(XRANGE/2):  # int(XRANGE/2)
            toplist.append(d+(p-1)-XRANGE-1)  # XRANGE-1
        else:
            toplist.append(d+top-int(XRANGE/2))  # int(XRANGE/2)


count1 = 0
count2 = 0

while(True):

    data = camera.get_video_data()

    dataB = data[0::3]
    dataG = data[1::3]
    dataR = data[2::3]

    # print(data)
    h = camera_info['MaxHeight']
    w = camera_info['MaxWidth']
    imgR = np.frombuffer(dataR, dtype='uint8').reshape((h, w))
    imgG = np.frombuffer(dataG, dtype='uint8').reshape((h, w))
    imgB = np.frombuffer(dataB, dtype='uint8').reshape((h, w))
    img = cv2.merge((imgB, imgG, imgR))

    #cv2.imshow('test', img)

    #ret, frame = img.read()
    gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)

    key = cv2.waitKey(1)
    if key == ord('s'):
        reset_standard()
        color1 = np.array([255., 255., 255.])
        for tyouten in basetop2:  # basetop[::100]:
            yy = int((tyouten//wide)+1)
            xx = int(tyouten-(wide*(yy-1)))
            #print(xx,yy)
            cv2.circle(img=avg, center=(xx, yy), radius=5,
                       color=color1, thickness=2, lineType=cv2.LINE_AA)

    if key == ord('b'):
        print("なし", count1, count2, count1/count2)
        print("吹いた")
        count1 = 0
        count2 = 0
    if key == ord('e'):
        print("止めた")
        print("あり", count1, count2, count1/count2)
        count1 = 0
        count2 = 0

    data1 = gray.copy().astype("float")
    data1 = np.asarray(data1)
    #print(data1)
    data1 = data1.flatten()  # 元データ
    #print(data1)

    
    if avg is None:
        reset_standard()
        color1 = np.array([255., 255., 255.])

        for tyouten in basetop2:  # basetop[::100]:
            yy = int((tyouten//wide)+1)
            xx = int(tyouten-(wide*(yy-1)))
            #print(xx,yy)
            cv2.circle(img=avg, center=(xx, yy), radius=5,
                       color=color1, thickness=2, lineType=cv2.LINE_AA)
        continue

    searching_top()

    if bef_toplist is None:
        bef_toplist = toplist
        continue

    now_toplist = toplist

    

    color2 = np.array([255., 255., 255.])
    sa_x = 0
    sa_xx = 0
    for iti in range(0, len(basetop2)):
        sa = bef_toplist[iti]-now_toplist[iti]
        sa_x += sa
        sa_xx += sa**2
        #print(basetop2[iti],toplist[iti])
        sa = int(abs(sa)*100)
        #print(sa)
        #print(sa)
        bt1 = basetop2[iti]
        #print(sa)
        #if sa>=1.0:
        yy2 = int((bt1//wide)+1)
        xx2 = int(bt1-(wide*(yy2-1)))
        #print(xx2, yy2)
        if sa < 10000:
            cv2.circle(img=gray, center=(xx2, yy2), radius=sa,
                       color=color2, thickness=2, lineType=cv2.LINE_AA)

    #print(len(basetop2))#880
    try:
        sa_x2 = (sa_x/len(basetop2))**2  # 合計の平均の２乗
        sa_xx2 = sa_xx/len(basetop2)  # 2乗合計の平均
        ss = sa_xx2-sa_x2
        if bef_ss == None:
            bef_ss = ss
        now_ss = ss
        #n=880,880
        #分子の自由度879,分母の自由度879
        #
        #パーセント点
        #https://keisan.casio.jp/exec/system/1161228871
        #1%基準
        #1.17006201074148931468
        #0.1%基準
        #1.232087743495074646261
        #0.05%基準
        #1.248901820536719497381
        per = 1.2489

        if bef_ss >= now_ss:
            F = bef_ss/now_ss  # round(bef_ss/now_ss,5)
        else:
            F = now_ss/bef_ss  # round(now_ss/bef_ss, 5)
            #a = round(2 / 3, 5)小数点第５位まで
        #print(F)
        if F >= 2 or F == "inf":
            pass
        else:
            #print(F)
            count2 += 1
            if F > per:
                #print(F)
                #print("違う")
                count1 += 1
    except:
        now_ss = 1
        pass

    cv2.imshow("image2",  cv2.convertScaleAbs(avg))  # 前
    cv2.imshow("now", gray)  # 今

    bef_toplist = now_toplist
    bef_ss = now_ss

    time.sleep(0.01)

    if key == 27:
        break

    