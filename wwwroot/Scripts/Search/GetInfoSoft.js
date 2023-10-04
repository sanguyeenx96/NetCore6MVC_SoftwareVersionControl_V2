$(".btnXemchitiet").click(function () {
    function formatDate(dateString) {
        const options = { year: "numeric", month: "2-digit", day: "2-digit" };
        return new Date(dateString).toLocaleDateString("vi-VN", options);
    }

    var name = $(this).data("name");
    var version = $(this).data("version");
    var diemthaydoi = $(this).data("diemthaydoi");
    var ngayapdung = $(this).data("ngayapdung");
    var nguoicaidat = $(this).data("nguoicaidat");
    var ngaycaidat = $(this).data("ngaycaidat");
    var soluongjigcaidat = $(this).data("soluongjigcaidat");
    var nguoixacnhan = $(this).data("nguoixacnhan");
    var trangthaiapdung = $(this).data("trangthaiapdung");
    var isTrangThaiApDung = ["true", "1", "yes"].includes(trangthaiapdung.toLowerCase());

    var path = $(this).data("path");
    var timecreated = $(this).data("timecreated");

    var tableHtml =
        '<table class="table table-striped" style="font-size: small;">';
    tableHtml += "<tbody>";
    tableHtml += "<tr><td>Version</td><td>" + version + "</td></tr>";
    tableHtml +=
        "<tr><td>Trạng thái áp dụng</td><td>" +
    (isTrangThaiApDung
            ? '<span style="width:auto" class="badge badge-success">Đang áp dụng</span>'
        : '<span style="width:auto" class="badge badge-danger">Không áp dụng</span>') +
        "</td></tr>";
    tableHtml += "<tr><td class='col-4'>Điểm thay đổi</td><td>" + diemthaydoi + "</td></tr>";
    tableHtml +=
        "<tr><td>Ngày Áp Dụng</td><td>" + formatDate(ngayapdung) + "</td></tr>";
    tableHtml +=
        "<tr><td>Ngày Cài Đặt</td><td>" + formatDate(ngaycaidat) + "</td></tr>";
    tableHtml +=
        "<tr><td>Số Lượng Jig Cài Đặt</td><td>" +
        soluongjigcaidat +
        "</td></tr>";
    tableHtml += "<tr><td>Tên phần mềm</td><td>" + name + "</td></tr>";
    tableHtml += "<tr><td>File Name</td><td>" + path + "</td></tr>";
    tableHtml += "<tr><td>Người Cài Đặt</td><td>" + nguoicaidat + "</td></tr>";
    tableHtml +=
        "<tr><td>Người Xác Nhận</td><td>" + nguoixacnhan + "</td></tr>";
    tableHtml += "<tr><td>Upload At</td><td>" + timecreated + "</td></tr>";
    tableHtml += "</tbody></table>";

    Swal.fire({
        icon: "info",
        title: name,
        html: tableHtml, // Sử dụng biến HTML chứa bảng
        position: "top-start",
        showClass: {
            popup: `
                        animate__animated
                        animate__fadeInLeft
                        animate__faster
                    `,
        },
        hideClass: {
            popup: `
                        animate__animated
                        animate__fadeOutLeft
                        animate__faster
                    `,
        },
        grow: "column",
        width: 800,
        showConfirmButton: false,
        showCloseButton: true,
    });
});
