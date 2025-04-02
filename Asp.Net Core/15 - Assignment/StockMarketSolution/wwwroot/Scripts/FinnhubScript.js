const token = document.querySelector("#FinnhubToken").value;
const socket = new WebSocket('wss://ws.finnhub.io?token=${token}');
var stockSymbol = document.getElementById("StockSymbol").value;

// Connection opened -> Subscribe
socket.addEventListener('open', function (event) {
    socket.Send(JSON.stringify({ 'type': 'subscribe', 'symbol': stockSymbol }))
});

// Listen for messages
socket.addEventListener('message', function (event) {
    if (event.data.type == "error") {
        $(".price").text(event.data.msg);
        return;
    }

    var eventData = JSON.parse(event.data);
    if (eventData) {
        if (eventData.data) {
            var updatedPrice = JSON.parse(event.data[0]).data[0].p;
            var timestamp = JSON.parse(event.data[0]).data[0].t;

            $(".price").text(updatedPrice.toFixed(2));
            $("#price").text(updatedPrice.toFixed(2));
        }
    }

});

// Unsubscribe
var unsubscribe = function (symbol) {
    socket.send(JSON.stringify({ 'type': 'unsubscribe', 'symbol': symbol }))
}

window.onunload = function () {
    unsubscribe(stockSymbol);
};