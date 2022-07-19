# ChaosRecipeFilter

Pretty old project from before the chaos recipe tool a lot of people use nowadays was around. Now that I went ahead and made a proper github account for myself, I might as well toss it in here. Despite it not being as feature rich as the other chaos recipe tool, I still use it since I try to keep my overlays as minimal as possible anyway and the other tool was a bit too overequipped for what I wanted.


Updates your filter file to show and highlight whatever items you need to do a chaos recipe with.    
Runs the /itemfilter command in game after execution if exactly one filter is being updated.   
-possible to have it look in production_Config.ini for the active filter name rather than using whatever youre working on    
--would require either another setting to know its location but I left it out since it would go unused (at least by myself)      
Currently does not look at any stash apis to determine which items are missing, you'll have to click the checkboxes manually.    


## To Use
-Build project and run it.    
-Click on the settings tab.    
-Input your border color, minimap color, and minimap icon preferences at the top.    
-Input the location of the filter file(s) you want to be modifying.    
-Input the text in the filter that is right above where the recipe filter lines should be inserted.    
--previously this is automated, but neversink alters the filter layout often enough that I found manual entry easier in the end since it was a once per league thing anyway.    
-Input the text in the filter that is right below where the recipe filter lines should be inserted.   
-Save settings and go back to the Filter Control tab.   
-Check all the item types you need to complete your recipes still.   
--note that weapons only target 1x3 1handers and 2x3 2handers to more efficiently fit in stash tabs.   
-Check the <75 box if you want to make sure these items won't make a regal orb.   
--only one item per recipe needs to be less than ilvl 75 to get chaos.   
-Execute.   
