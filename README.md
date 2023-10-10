# NetCore6MVC_SoftwareVersionControl_V2
Quán lý phiên bản các phần mềm trong công ty CEV
+ Lưu lịch sử thay đổi các phiên bản phần mềm
+ Không giới hạn upload
+ Upload, Download phiên bản phần mềm 
+ Tìm kiếm nhanh phần mềm
+ Sửa đổi thông tin
+ Quản lý lịch sử thao tác người dùng
+ Quản lý tài khoản người dùng, phân quyền tài khoản

# Cấu Trúc Cơ Sở Dữ Liệu
Dưới đây là mô tả cấu trúc cơ sở dữ liệu:

## Bảng `Lichsu`

- `Id`: Khóa chính của bảng, kiểu số nguyên.
- `Ten`: Tên.
- `Hanhdong`: Hành động.
- `Thoigian`: Thời gian.
- `Type`: Loại.

## Bảng `Model`

- `Id`: Khóa chính của bảng, kiểu số nguyên.
- `ParentModelId`: Khóa ngoại đến bảng `Model` (Model cha), kiểu số nguyên.
- `Name`: Tên Model (bắt buộc).
- `ChildrenModel`: Danh sách các Model con (ICollection<Model>).
- `ParentModel`: Model cha (Model navigation property).
- `SoftNames`: Danh sách các SoftName liên quan (ICollection<SoftName>).

## Bảng `Password`

- `Id`: Khóa chính của bảng, kiểu số nguyên.
- `Type`: Loại.
- `Username`: Tên người dùng.
- `Hoten`: Họ tên.
- `Pwd`: Mật khẩu.
- `Role`: Vai trò.

## Bảng `SoftName`

- `Id`: Khóa chính của bảng, kiểu số nguyên.
- `Name`: Tên.
- `ModelId`: Khóa ngoại đến bảng `Model` (Model cha), kiểu số nguyên.
- `Model`: Model liên quan (Model navigation property).
- `Softwares`: Danh sách các phần mềm liên quan (ICollection<Software>).

## Bảng `Software`

- `Id`: Khóa chính của bảng, kiểu số nguyên.
- `Version`: Số phiên bản (bắt buộc).
- `DiemThayDoi`: Điểm thay đổi.
- `NgayApDung`: Ngày áp dụng.
- `NguoiCaiDat`: Người cài đặt.
- `NgayCaiDat`: Ngày cài đặt.
- `SoLuongJigCaiDat`: Số lượng Jig cài đặt.
- `NguoiXacNhan`: Người xác nhận (bắt buộc).
- `TrangThaiApDung`: Trạng thái áp dụng (bắt buộc).
- `Path`: Đường dẫn đến tệp.
- `FileName`: Tên tệp.
- `TimeCreated`: Ngày giờ tạo.
- `SoftNameId`: Khóa ngoại đến bảng `SoftName`, kiểu số nguyên.
- `SoftName`: SoftName liên quan (SoftName navigation property).

# HÌnh ảnh Demo
![1](https://github.com/sanguyeenx96/NetCore6MVC_SoftwareVersionControl_V2/assets/103011257/fed93326-581e-4c1d-b322-58e9d8412598)

![2](https://github.com/sanguyeenx96/NetCore6MVC_SoftwareVersionControl_V2/assets/103011257/3cd1897c-9e4d-412d-88b2-4dd07c40c83a)

![3](https://github.com/sanguyeenx96/NetCore6MVC_SoftwareVersionControl_V2/assets/103011257/f6fafc48-cac9-4bf6-a5ae-14f25c978030)

![4](https://github.com/sanguyeenx96/NetCore6MVC_SoftwareVersionControl_V2/assets/103011257/cdab07fc-65ec-4d68-99ad-13161a1a5911)

![5](https://github.com/sanguyeenx96/NetCore6MVC_SoftwareVersionControl_V2/assets/103011257/05c5c9b3-2efc-4a87-942b-d32dbfc6de77)

![6](https://github.com/sanguyeenx96/NetCore6MVC_SoftwareVersionControl_V2/assets/103011257/a5dd3643-13b5-4b77-964a-81bf765ff3cc)

![7](https://github.com/sanguyeenx96/NetCore6MVC_SoftwareVersionControl_V2/assets/103011257/af153d20-460c-48f4-9293-7d6159fd56ed)

![a1](https://github.com/sanguyeenx96/NetCore6MVC_SoftwareVersionControl_V2/assets/103011257/90784beb-ebd4-401f-90d3-b34e23ca96be)

![a2](https://github.com/sanguyeenx96/NetCore6MVC_SoftwareVersionControl_V2/assets/103011257/529e2865-53f2-4581-a135-584ee5ddc45a)

![a3](https://github.com/sanguyeenx96/NetCore6MVC_SoftwareVersionControl_V2/assets/103011257/f1f1d15c-2b2a-4f21-8d44-cb4ddf95bf0f)

![a4](https://github.com/sanguyeenx96/NetCore6MVC_SoftwareVersionControl_V2/assets/103011257/24cded0b-fa23-4d04-84a3-e8dca99c2c3f)

![a5](https://github.com/sanguyeenx96/NetCore6MVC_SoftwareVersionControl_V2/assets/103011257/2acb5572-ce00-4b7b-85be-217bd58b1095)

![8](https://github.com/sanguyeenx96/NetCore6MVC_SoftwareVersionControl_V2/assets/103011257/46196867-f1fb-4d4a-b06b-139296e45b85)

![9](https://github.com/sanguyeenx96/NetCore6MVC_SoftwareVersionControl_V2/assets/103011257/b7dfaecd-2ad7-49e9-920d-2a3edb9184e9)

![10](https://github.com/sanguyeenx96/NetCore6MVC_SoftwareVersionControl_V2/assets/103011257/c1074b79-d2c2-41e5-ab02-308a0a38cb1e)
