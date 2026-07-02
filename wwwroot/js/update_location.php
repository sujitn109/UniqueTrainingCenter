<?php
header("Content-Type: application/json");
$conn = new mysqli("localhost", "root", "", "school_bus_db");

if ($conn->connect_error) {
    die(json_encode(["status" => "error", "message" => "Database Connection Failed"]));
}

$data = json_decode(file_get_contents("php://input"), true);

if (!empty($data['driver_id']) && !empty($data['latitude']) && !empty($data['longitude'])) {
    $driver_id = $data['driver_id'];
    $lat = $data['latitude'];
    $lng = $data['longitude'];
    $time = $data['timestamp'];

    // लोकेशन डेटाबेसमध्ये अपडेट किंवा इन्सर्ट करणे
    $query = "INSERT INTO bus_tracking (driver_id, latitude, longitude, updated_at) 
              VALUES ('$driver_id', '$lat', '$lng', '$time') 
              ON DUPLICATE KEY UPDATE latitude='$lat', longitude='$lng', updated_at='$time'";

    if ($conn->query($query)) {
        echo json_encode(["status" => "success", "message" => "Location updated"]);
    } else {
        echo json_encode(["status" => "error", "message" => $conn->error]);
    }
} else {
    echo json_encode(["status" => "error", "message" => "Invalid Data"]);
}
?>