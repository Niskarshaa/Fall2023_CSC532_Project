﻿using Fall2020_CSC403_Project.item_system.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fall2020_CSC403_Project.item_system
{
    public class ItemFactory
    {
        
        public IItem GetItem(string itemType, FrmLevel frmLevel)
        {

            switch (itemType) 
            {
                case "PeanutPotion":

                    return new PeanutPotion(frmLevel);

                case "WallBoom":
                    return new WallBoom();

                case "Enemy1Boom":
                    return new Enemy1Boom();


                default:
                    throw new ArgumentException("invalid item type");
            }

        }
    }
}