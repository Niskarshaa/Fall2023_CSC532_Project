using Fall2020_CSC403_Project.code;
using Fall2020_CSC403_Project.item_system.interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fall2020_CSC403_Project.item_system
{
    public class ItemFactory
    {
        int NUM_ITEMS = 0;
        public IItem GetItem(int itemID, FrmLevel frmLevel, float locationX, float locationY)
        {

            switch (itemID) 
            {
                case 1: // "WallBoom"
                    NUM_ITEMS += 1;
                    return new WallBoom(frmLevel, NUM_ITEMS, locationX, locationY);

                case 2: //"Enemy1Boom"
                    return new CheetoBoom(frmLevel, NUM_ITEMS, locationX, locationY);

                case 3: //"RedPill"
                    return new RedPill(frmLevel, NUM_ITEMS, locationX, locationY);


                default:
                    throw new ArgumentException("invalid item type");
            }

        }

    }
}
