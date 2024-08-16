window.addEventListener("load", () => {
    const qrCodeDataElements = document.getElementsByClassName("qrCodeData");
    const qrCodeElements = document.getElementsByClassName("qrCode");

    for (let i = 0; i < qrCodeDataElements.length; i++) {
        const uri = qrCodeDataElements[i].getAttribute('data-url');
        new QRCode(qrCodeElements[i], {
            text: uri,
            width: 150,
            height: 150
        });
    }
});