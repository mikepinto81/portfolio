<?php
//*****************************************
//Submit data to BSD and return any errors
//Author: Mike Pinto (mike@zotnip.com)
//*****************************************

//setup variables
$ZIP = "";
$EMAIL = "";

//fill variables from POST
if(isset($_POST['ZIP']))
	$ZIP = $_POST["ZIP"];
if(isset($_POST['EMAIL']))
	$EMAIL = $_POST["EMAIL"];
			
//create XML string data	
$xmlData = 
"<?xml version='1.0' encoding='UTF-8'?>
<api>
<signup_form id='69'>
<signup_form_field id='840'>".$EMAIL."</signup_form_field>
<signup_form_field id='849'>".$ZIP."</signup_form_field>			
<source>UFCWWest.org Website form</source>
</signup_form>
</api>";

//create xml object from xml string
$xml = new SimpleXMLElement($xmlData);

//variables to submit to api
$apiID = "formpost";
$timeStamp = time();
$pageUrl = "/page/api/signup/process_signup";
$params = "api_ver=2&api_id=formpost&api_ts=" . $timeStamp;
$signingString = $apiID . "\n" . $timeStamp . "\n" . $pageUrl . "\n" . $params;
$apiSecret = "GETSECRETFROMBSDBACKEND";
$apiMac = hash_hmac('sha1', $signingString, $apiSecret);

//put together a request url
$assembledRequest = "http://action.ufcwwest.org" . $pageUrl . "?" . $params . "&api_mac=" . $apiMac;

//use curl and send request 
$ch = curl_init($assembledRequest);
curl_setopt($ch, CURLOPT_POST, 1);
curl_setopt($ch, CURLOPT_POSTFIELDS, $xml->asXML());
curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);

$response = curl_exec($ch);
curl_close($ch);

//will show error codes or blank for success
echo $response;
?>