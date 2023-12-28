using System.Collections.Generic;

public class Program {
    public static void Main() {
        string line;
        int T = int.Parse(System.Console.ReadLine ());
        List<int[]> nums=new List<int[]>();
        int[] sum=new int[T];
        for(int i=0;i<T;i++){
            int n=int.Parse(System.Console.ReadLine ());
            //将每行的数据读取存为数组
            int[] num=new int[n];
            line=System.Console.ReadLine ();
            string[] str=line.Split();
            for(int j=0;j<n;j++){
                num[j]=int.Parse(str[j]);
                sum[i]+=num[j];
            }
            nums.Add(num);
        }
    
    //找到偶数最小除以2得到奇数的操作数
        
        for(int i=0;i<T;i++){
            int[] num=nums[i];
            //如果求和为偶数,直接输出0
            
            if(sum[i]%2==0){
                System.Console.WriteLine(0);
                continue;
            }
            int min=100000;
            int count=0;
            for(int j=0;j<num.Length;j++){
                if(num[j]%2==0){
                    count=0;
                    while(num[j]%2==0){
                        num[j]/=2;
                        count++;
                    }
                    if(count<min){
                        min=count;
                    }
                }
            }
            
            System.Console.WriteLine(min);
        }
    
    }
}