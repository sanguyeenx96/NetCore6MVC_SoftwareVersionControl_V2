$("#btnMatkhau").click(function () {
    var pwd = $(this).data("pwd");
    var id = $(this).data("id");
    Swal.fire({
        title: "Đổi mật khẩu tài khoản",
        html: `
            <input type="password" id="pwdcu" class="swal2-input" placeholder="Nhập mật khẩu hiện tại" autocomplete="off" >
            <input type="password" id="pwdmoi" class="swal2-input" placeholder="Nhập mật khẩu mới" autocomplete="off" >
            `,
        confirmButtonText: "Xác nhận",
        focusConfirm: false,
        preConfirm: () => {
            const pwdcu = Swal.getPopup().querySelector("#pwdcu").value;
            const pwdmoi = Swal.getPopup().querySelector("#pwdmoi").value;
            if (!pwdcu || !pwdmoi) {
                Swal.showValidationMessage(`Phải nhập đầy đủ thông tin`);
            }
            if (pwdcu != pwd) {
                Swal.showValidationMessage(`Mật khẩu hiện tại không đúng`);
            }
            return { pwdmoi: pwdmoi };
        },
    }).then((result) => {
        var pwdmoi = result.value.pwdmoi.trim();
        $.ajax({
            url: "/User/Doimatkhau",
            type: "POST",
            data: {
                id: id,
                pwd: pwdmoi,
            },
            success: function (data) {
                if (data.success) {
                    Swal.fire(
                        "Thành công!",
                        "Bạn vừa đổi mật khẩu thành công",
                        "success"
                    ).then(function () {
                        window.location.reload();
                    });
                } else {
                    Swal.fire("Thất bại!", "Lỗi kết nối", "error");
                }
            },
            error: function (xhr, t, error) {
                Swal.fire("Thất bại!", "Lỗi kết nối", "error");
            },
        });
    });
});
