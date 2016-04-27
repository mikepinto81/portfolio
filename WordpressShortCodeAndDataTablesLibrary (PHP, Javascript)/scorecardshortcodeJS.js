var sc_table;

jQuery(document).ready( function () {
		var mainTable = jQuery('#MainTable');
		if(mainTable == null)
			return;
				
		//first set the main table to the first tables data
		jQuery('#MainTable').html(jQuery('#firsttable').html());
		
		//use datatables library to turn MainTable into pretty datatable
		sc_table = jQuery('#MainTable').DataTable({
			responsive:true			
			
			}
		);		
			
	} 	
	
);

function LoadNewTable(newID)
{			
	//destroy the current datatable.
	sc_table.destroy();
	
	//put the html into the main table from input table
	jQuery('#MainTable').html(jQuery('#'+newID).html());
	
	//load datatable 
	sc_table = jQuery('#MainTable').DataTable({
		responsive:true		
		}
	);
	
	//scroll to the table
	ScrollToTable();
}

function ScrollToTable()
{
	var target = jQuery("#MainTable_wrapper");      
	
	jQuery('html,body').animate({
		scrollTop: target.offset().top - 200
	}, 800);
	
	
}
