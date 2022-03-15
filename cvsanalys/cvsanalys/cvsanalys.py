import csv


csvfile = open(
    r"C:\Users\mayuk\source\repos\MusicPlayingDandelion\cvsdata\masukuCVS013.csv", 'r')
reader = csv.reader(csvfile)

#print(reader)

#全体の平均(マスクあり)　24.71064217

#全体の平均(マスクなし)　35.97260743

withmask = 24.71064217**2

nomask = 35.97260743**2


for row in reader:
    # print(row)
    frequency = float(row[0])
    quantity = float(row[1])
    #print(frequency)

    sum = 0
    sqsum = 0

    cou = 0

    for row in reader:
        print(row)
        frequency = float(row[0])
        quantity = float(row[1])
        sum += frequency
        sqsum += frequency**2
        print("cou", cou)
        cou += 1

    #print("できた")
    #print("cou",cou)
    #print("sum",sum)
    avesum = (sum/cou)**2  # 合計の平均の２乗

    avesqsum = sqsum/cou  # 2乗合計の平均

    print("二乗の合計", sqsum)

    variance = avesqsum-avesum  # 分散
    print("variance", variance)

    deviation = variance**0.5
    print("standerd deviation", deviation)


#analys(reader)
