using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class DTRoleAnimCategoryEntity
{
    public int Attack(int param)
    {
        int res = 0;
        switch (param)
        {
            case 1:
                res = Attack_1;
                break;

            case 2:
                res = Attack_2;
                break;

            case 3:
                res = Attack_3;
                break;
        }

        return res;
    }
}