using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testLeetcode : MonoBehaviour
{
    // Start is called before the first frame update
    public int x = 10;
    public Solution solution=new Solution();
    void Start()
    {
        Debug.Log("Ans:"+solution.ArrayNesting(new int[]{5,4,0,3,1,6,2}));
    }
    [ContextMenu("LeetCode/Do")]
    public void Do(){
       
        
    }

}

public  class Solution {
  public int ArrayNesting(int[] nums) {
        int i =0;
        int len = nums.Length;
        int MaxNum=0;

        while(i!=len-1)
        {   
            Debug.Log("i:"+i);
            if(nums[i]==-1){
            i++;
            continue;}
            Debug.Log("i2:"+i);
            int start_index=i;
            int point=nums[i];
            int CircNum=0;
            int temp=0;
            if(point==i){i++;
                if(MaxNum==0) MaxNum=1;
             continue;}
            while( point!=start_index ){ 
                Debug.Log("point:"+point);
                Debug.Log("nums[point]:"+nums[point]);
                CircNum++;
                temp=point;
                point=nums[point];
                Debug.Log("new point:"+point);
                nums[temp]=-1;
            }
            if(CircNum>MaxNum) MaxNum=CircNum;
            
            i++;
        }
        return MaxNum+1;

    }
}