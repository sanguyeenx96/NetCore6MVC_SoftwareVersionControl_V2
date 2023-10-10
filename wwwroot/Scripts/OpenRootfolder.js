$("#btnRootfolder").click(function () {
    $.ajax({
        type: "GET",
        url: "/Home/OpenFolderInWWWRoot",
        success: function (result) {
            if (result.success) {
                Swal.fire({
                    icon: "info",
                    title: "Đường dẫn tới thư mục chứa phần mềm tại máy chủ 192.168.1.254",
                    text: result.data,
                });
            }
        },
        error: function () {
            console.log("Đã xảy ra lỗi khi gọi API.");
        },
    });
});
