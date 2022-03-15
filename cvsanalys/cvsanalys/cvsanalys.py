import csv


csvfile = open(r"C:\Users\mayuk\source\repos\MusicPlayingDandelion\cvsdata\nothingCVS001.csv", 'r')
reader = csv.reader(csvfile)

rlist = []

for row in reader:
    eL = 0
    eH = 0
    r = 0
    for a in row[1:17]:
        eL += float(a)**2

    for b in row[17:33]:
        eH += float(b)**2

    r = eH/eL
    rlist.append(r)
print(rlist)

sum = 0
sqsum = 0

for c in rlist:
    sum +=c
    sqsum +=c**2


cou=len(rlist)
ave=sum/cou #平均値
avesum = (sum/cou)**2  # 合計の平均の２乗
avesqsum = sqsum/cou  # 2乗合計の平均

variance = avesqsum-avesum  # 分散
deviation = variance**0.5
print("平均値",ave)
print("分散", variance)
print("標準偏差",deviation)





#def analys():
    #frequency = float(row[0])
    #quantity = float(row[1])
    #print(frequency)

    #sum = 0
    #sqsum = 0

    #cou = 0

    #for row in reader:
        #print(row)
        #frequency = float(row[0])
        #quantity = float(row[1])
        #sum += frequency
        #sqsum += frequency**2
        #print("cou", cou)
        #cou += 1

    #print("できた")
    #print("cou",cou)
    #print("sum",sum)
    #avesum = (sum/cou)**2  # 合計の平均の２乗

    #avesqsum = sqsum/cou  # 2乗合計の平均

    #print("二乗の合計", sqsum)

    #variance = avesqsum-avesum  # 分散
    #print("variance", variance)

    #deviation = variance**0.5
    #print("standerd deviation", deviation)
