<<<<<<< HEAD
import csv


csvfile = open(r"C:\Users\mayuk\source\repos\cvsanalys\SaveData002.csv", 'r')
reader = csv.reader(csvfile)

sum=0
sqsum=0



for row in reader:    
    print(row)

    frequency = float(row[0])
    quantity = float(row[1])
    

    sum+=quantity
    sqsum+=quantity**2

    if  frequency==255:
        break

avesum=(sum/256)**2 #合計の平均の２乗
avesqsum=sqsum/256 #2乗合計の平均

variance= avesqsum-avesum #分散
print("variance",variance)

=======
import csv


csvfile = open(r"C:\Users\mayuk\source\repos\cvsanalys\SaveData002.csv", 'r')
reader = csv.reader(csvfile)

sum=0
sqsum=0



for row in reader:    
    print(row)

    frequency = float(row[0])
    quantity = float(row[1])
    

    sum+=quantity
    sqsum+=quantity**2

    if  frequency==255:
        break

avesum=(sum/256)**2 #合計の平均の２乗
avesqsum=sqsum/256 #2乗合計の平均

variance= avesqsum-avesum #分散
print("variance",variance)

>>>>>>> a281ac08ad1ad89ddc1aebc9e566386f8f615d9d
