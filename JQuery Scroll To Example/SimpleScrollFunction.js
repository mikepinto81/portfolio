//last shown item ID
var lastScrolledItemID = "";

//Swaps contents of a the "InfoBarArea" and then scrolls to it.
/*
* id : HTML element ID
* newbakcolor : color HEX value
* scrollToArea : bool
*/
function ShowHomeInnerInfoItem(id, newbakcolor, scrollToArea)
{
    //hide last item then show new item
    if(lastScrolledItemID != "")
    {
        jQuery("#"+lastScrolledItemID).hide();						
    }
    
    //show new item
    jQuery("#"+id).show();						
    lastScrolledItemID = id;
    
    //change bak color
    jQuery("#InfoBarArea").css("background-color",newbakcolor); 
    
    //scroll to area
    if(scrollToArea)
    {
        jQuery('html, body').animate({
            scrollTop: jQuery("#InfoBarArea").offset().top - 180
        }, 500);
    }
}
