function downloadFile(filePath) {
    $.ajax({
        type: "GET",
        url: "/File/DownloadFile?filename=" + encodeURIComponent(filePath),
        xhrFields: {
            responseType: "blob", // Đặt kiểu dữ liệu của phản hồi thành blob
        },
        success: function (data) {
            var url = window.URL.createObjectURL(data);
            // Tạo một thẻ a ẩn để tải file
            var a = document.createElement("a");
            a.href = url;
            a.download = filePath; // Tên file khi tải về
            // Thêm thẻ a vào body và kích hoạt sự kiện click để tải file
            document.body.appendChild(a);
            a.click();
            // Xóa đường dẫn URL sau khi đã tải xong
            window.URL.revokeObjectURL(url);
            Swal.fire("Thành công", "Tập tin đang được tải về...", "success");
        },
        error: function () {
            Swal.fire("Thất bại", "Không tìm thấy tập tin ", "error");
        },
    });
}
//Button Download
$(".btnDownload").on("click", function (e) {
    e.preventDefault();
    e.stopPropagation();
    var id = $(this).data("id");
    var name = $(this).data("name");
    var path = $(this).data("path");
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: "btn btn-success",
            cancelButton: "btn btn-danger",
        },
        buttonsStyling: true,
    });
    swalWithBootstrapButtons
        .fire({
            title: name,
            icon: "question",
            showCancelButton: true,
            confirmButtonText:
                '<i class="fa fa-download fa-fw"></i> Tải Version mới nhất',
            cancelButtonText: "Tải Version cũ",
            reverseButtons: true,
        })
        .then((result) => {
            if (result.isConfirmed) {
                downloadFile(path);
            } else if (result.dismiss === Swal.DismissReason.cancel) {
                $.ajax({
                    url: "/Soft/GetSelectListVersion",
                    method: "GET",
                    data: { id: id },
                    success: function (data) {
                        console.log(data);
                        const vers = data;
                        const inputOptions = {};
                        vers.forEach((ver) => {
                            inputOptions[ver.path] = ver.version;
                        });
                        swalWithBootstrapButtons
                            .fire({
                                title: "Chọn Version",
                                input: "select",
                                inputOptions: inputOptions,
                                showCancelButton: true,
                                confirmButtonText:
                                    '<i class="fa fa-download fa-fw"></i> Tải về',
                                cancelButtonText: "Huỷ bỏ",
                                reverseButtons: true,
                            })
                            .then((result) => {
                                if (result.isConfirmed) {
                                    var path = result.value;
                                    downloadFile(path);
                                } else if (
                                    result.dismiss === Swal.DismissReason.cancel
                                ) {
                                    swalWithBootstrapButtons.fire(
                                        "Đã huỷ",
                                        "Thao tác đã được huỷ bỏ!",
                                        "error"
                                    );
                                }
                            });
                    },
                    error: function () {
                        Swal.fire(
                            "Lỗi máy chủ",
                            "Không tải được dữ liệu",
                            "error"
                        );
                    },
                });
            }
        });
});
