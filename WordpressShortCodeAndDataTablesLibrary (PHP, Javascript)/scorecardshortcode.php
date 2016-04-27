<?php 
// Add Shortcode
function scorecarddatashortcode($atts) {
		
	// Attributes
	$args = shortcode_atts( 
    array(
        'years' => '2014',        
    ), $atts );	
		
	//split years by space into array	
	$yearsArray = explode(" ",$args['years']);
	
	//setup variables	
	$html = ''; //return html	
	$count = 0; 
		
	//*************************
	//make buttons	
	foreach($yearsArray as $singleYear){
		$html .= '<div class="singleYear">';
			$html .= '<h2>' . $singleYear . '</h2>';
			
			// WP_Query arguments
			$args = array (
				'post_type'              => array( 'datatable' ),
				'post_status'            => array( 'publish' ),
				'posts_per_page'         => '-1',
				'order'                  => 'ASC',
				'orderby'                => 'title',
				'date_query' => array(
				array(
					'year'  => $singleYear,					
				),
				)
			);

			// The Query
			$query = new WP_Query( $args );
			
			$html .= '<ul style="" class="scoreCardButtons">';
			
			// The Loop
			if ( $query->have_posts() ) {
				while ( $query->have_posts() ) {
					$query->the_post();
					
					$slug = ( basename(get_permalink()) );
					$slug = str_replace("?datatable=","",$slug);
					if($count == 0)
						$slug = "firsttable";
					$count++;
					
					$html .= '<li>';
					$html .= '<button onclick="LoadNewTable(\''.$slug.'\');">'. get_the_title() .'</button>';
					$html .= '</li>';
				}
			} 

			$html .= '</ul>';
			
			// Restore original Post Data
			wp_reset_postdata();
		
		$html .= '</div>'; //.singleYear div
	} //year loop
	
	
	//*********************************
	//make tables
	$count = 0;
	
	//main table will hold the currently selected table...other tables will be hidden and used as data holders
	$html .='<table id="MainTable" class = "display" style = "width:100%"><thead><tr><th></th></tr></thead><tbody><tr><td></td></tr></tbody></table>';
	
	foreach($yearsArray as $singleYear){
	
		// WP_Query arguments
		$args = array (
			'post_type'              => array( 'datatable' ),
			'post_status'            => array( 'publish' ),
			'posts_per_page'         => '-1',
			'order'                  => 'ASC',
			'orderby'                => 'title',
			'date_query' => array(
			array(
				'year'  => $singleYear,					
			),
		),
		);

		// The Query
		$query = new WP_Query( $args );

		// The Loop
		if ( $query->have_posts() ) {
			while ( $query->have_posts() ) {
				$query->the_post();
				
				$dataName = information_get_meta( 'information_data_name' );
				$csvLink =  information_get_meta( 'information_link_to_data_csv_file' );
				
				//get the id of the first table...will be used to make the first one active
				$slug = ( basename(get_permalink()) );
				$slug = str_replace("?datatable=","",$slug);
				if($count == 0)
					$slug = "firsttable";
				$count++;
				
				$html .= '<table id="' . $slug . '" style="display:none">';
				$f = fopen($csvLink, "r");
				if(!$f)
				{				
					fclose($f);
					continue;
				}
				$isHead = true;
				while (($line = fgetcsv($f)) !== false) {
						if($isHead)
						{
							$html .= '<thead>';
							$html .= "<tr>";
							foreach ($line as $cell) {
									$html .= "<th><div class='dataTableHeadTh'>" . htmlspecialchars($cell) . "</div></th>";
							}
							$html .= "</tr></thead><tbody>";
							$isHead = false;
						} else {
						$html .= "<tr>";
						foreach ($line as $cell) {
								$tdContent = htmlspecialchars($cell);
								$lowerContent = strtolower($tdContent);
								if( $lowerContent === "no")
									$html .= "<td bgcolor = '#dd0806'>" . $tdContent . "</td>";
								else if($lowerContent === "aye" || $lowerContent === "yes")
									$html .= "<td bgcolor = '#339966'>" . $tdContent . "</td>";
								else
									$html .= "<td>" . $tdContent . "</td>";
									
						}
						$html .= "</tr>";
					}
				}
				fclose($f);
				$html .= "</tbody>\n</table>";
				
				
			}
		} 
		
		// Restore original Post Data
		wp_reset_postdata();
		
	} //year foreach loop
		
		
		
	
	return $html;
	
	
}

add_shortcode( 'ShowScorecard', 'scorecarddatashortcode' ); 

?>