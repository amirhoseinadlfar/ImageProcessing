# توضیحات پروژه پردازش تصویر
## مشخصات پروژه:
فریم ورک استفاده شده : .Net 7

فریم ورک ظاهر کاربری : WPF

پکیج پردازش تصویر : System.Drawing.Comman

درس : مباحث پیشرفته 
## هدف از انجام پروژه
استفاده از سی شارپ برای پردازش تصاویر و همینطور پردازش چند هسته ای
## پنجره ها
پروژه دارای دو پنجره می باشد. یکی صفحه اصلی برای پردازش و ثبت تنظیمات پردازش و یکی صفحه تنظیم افکت برای اضافه کردن,حذف و نمایش اطلاعات مجموعه افکت ها
## روش کار کردن برنامه 
برنامه به این صورت ساخته شده که به خودی خود هیچ الگوریتمی برای پردازش تصاویر ندارد و عملکرد برنامه به این صورت است که الگوریتم ها را از اسمبلی یا کتابخانه های جداگانه روی برنامه بارگزاری میشوند, برنامه آنها را میخاند و با ظاهر کاربری به کاربر کمک میکند که تنظیمات مورد پسند خود را بر روی افکت ها یا الگوریتم ها اعمال کند.

برای این کار, پروژه دارای یک کتابخانه پایه برای ساختن کتاب خانه های پردازش تصویر بر پایه آن است.

این کتاب خانه Interface یی به نام IEffectBase را در خود دارد, و الگوریتم ها و افکت ها باید از آن پیروی کنند تا برنامه با انها کار کنند

این Interface قابلیت های زیر را برای افکت فراهم میکند


1.	تصویر ورودی که باید افکت بر آن اعمال شود
2.	تصویر خروجی که افکت باید نتیجه عملیات خود را در آن ذخیره کند
3.	متد آماده سازی که قبل از شروع پردازش ها فراخوانی میشود ( که در آن حتما باید تصویر خروجی تنظیم شود. هدف از این کار این است که افکت به راحتی بتواند در اندازه تصویر هم دخالت کند ) و افکت میتواند قبل از شروع پردازش ها یک سری پردازش ها را در آن ذخیره کند و از تکرار پردازشی که در همه پردازش مقدار یکسان دارند جلو گیری شود

البته این Interface به تنهایی قابلیت پردازش را ایجاد نمیکند و قابلیت پردازش با Interface دیگری با همین نام که از این Interface ارث بری میکند پشتیبانی میشود با این تفاوت که یک آرگومان Generic دارد و یک متد برای پردازش را فراهم میکند

دلیل استفاده از این Interface دوم این است که بتوان نوع ورودی متد پردازش را تغییر داد. نوع ورودی متد پردازش در حال حاضر به دو صورت زیر پشتیبانی میشود:

1.  AreaInfo : این ورودی برای پردازش یک منطقه از عکس و گزارش پیشرفت پردازش استفاده می شود. این نوع ورودی ممکن است نسبت به نوع دیگر سرعت بیشتری داشته باشد
2.  Vector2D<uint> یا Pixel : این نوع برای راحتی تعبیه شده و به این صورت می باشد که برای پردازش هر پیکسل یک بار متد پردازش فرخوانی میشود و نیازی به نوشتن حلقه های تو در تو برای پیمایش تصویر نیست و همینطور دیگر نیازی به گزارش پیشرفت نمی باشد.

با استفاده از این دو می توان بر روی تصویر پردازش کرد و خروجی مورد نظر را دریافت کرد. تصویر همیشه به شکل ماتریس می باشد که دارای 3 بعد دارد. بعد اول برای طول بعد دوم برای عرض و بعد سوم رنگ می باشد. بعد سوم همیشه با طول 4 و به صورت ARGB می باشد

به جز این Interface ها یک سری کلاس کمکی برای راحت کردن کار با افکت ها وجود دارند مانند کلاس های `PixelEffect`  و `AreaEffect` که نیاز به استفاده مستقیم از `IEffectBase` را از بین میبردو همینطور کلاس `Extentions. ImageProcessing.Helpers` که متد های `CloneEmpty` و `CloneWithAlpha` را برای استفاده در متد آماده سازی را فراهم می کنند که به شکل زیر استفاده می شوند:
```
Output = Image.CloneEmpty();
Output = Image.CloneWithAlpha();
```
به جز خواندن افکت ها یا الگوریتم ها, برنامه اطلاعاتی مانند نام توضیحات, متغیر ها را نیز از کتابخانه بارگزاری میکند که این کار به کمک الگوریتم ها انجام می شود. برای اینکه برنامه کتابخانه را به خوبی شناسایی کند, کتابخانه باید این Attribute را داشته باشد:
```
ImageProcessing.Base.Attributes.EffectAssemblyInfoAttribute
```
![image](https://github.com/amirhoseinadlfar/ImageProcessing/assets/56865457/1eee6d99-01a3-4690-9f61-8a1d99fd7526)

بعد از بارگزاری کتابخانه برنامه این اطلاعات را به نمایش میگذارد

![image](https://github.com/amirhoseinadlfar/ImageProcessing/assets/56865457/bd6b34b8-7d20-457a-bffc-c9055e1bb67d)

در مورد الگوریتم ها نیز باید از یک سری Attribute ها استفاده کرد
1.	`ImageProcessing.Base.Attributes.EffectAttribute` : که برای شناسایی افکت و نام آن توسط کتابخانه استفاده می شود
2.	`System.ComponentModel.DescriptionAttribute` (اختیاری) : که برای توضیحات افکت و توضیحات متغیر های افکت استفاده می شود
3.	`ImageProcessing.Base.Attributes. EffectPropertyAttribute` : برای ثبت متغیر هایی که باید توسط کاربر ثبت شوند استفاده شوند
![image](https://github.com/amirhoseinadlfar/ImageProcessing/assets/56865457/6d92e5d0-adbe-4a23-aa21-93f969667430)

که به این شکل به کاربر نمایش داده میشود
![image](https://github.com/amirhoseinadlfar/ImageProcessing/assets/56865457/7c42c384-2471-4be2-9477-a4017d3e9aa7)
در صورت استفاده از Description برای متغیر, کاربر میتواند آن را به صورت Tooltip ببیند
## پردازش چند هسته ای 
یکی از اهداف برنامه این بود که به راحتی و بدون نیاز به برنامه نویسی اضافه, قابلیت پردازش چند هسته ای در دسترس باشد. و این باعث می شود برنامه بتواند از افکت ها به صورت چند هسته ای و با سرعت بالا استفاده کند بدون اینکه کتاب خانه به خودی خود از آن پشتیبانی کند

البته یک سری موراد وجود دارند که باید توسط کتاب خانه رعایت شده باشد تا پردازش به درستی انجام شود به عنوان مثال:
1.	هر افکت باید روی بخشی نتیجه پردازش را ذخیره کنند که برای آنها در ورودی متد پردازش مقرر شده ( افکت می تواند به هر بخش دلخواه عکس دسترسی پیدا کند به عنوان مثال افکت بلور می تواند همه پیکسل های دور و اطراف پیکسل مورد نظر را بخواند حتی اگر خارج از محدوده ای که در ورودی متد پردازش بر آن مقرر شده باشد ولی فقط میتواند در محدودی ای که مقرر شده نتایج پردازش را ذخیره کند در غیر این صورت نتیجه پردازش بیهوده بود و ممکن است توسط پردازش هایی که در حال اجرا به صورت چند هسته و موازی هستند باز نویسی شوند )
2.	تغییرات در تصویر ورودی هیچ تاثیری بر برنامه اصلی ندارد ولی میتواند باعث مشکل عملکر پردازش ها به صورت موازی شود و خروجی مورد نظر به دست نیاید

یکسری تنظیمات پردازش چند هسته ای می تواند توسط کاربر تعیین شود مانند متد پردازش (Task ,Thread) که معمولا Task سریع تر است و همینطور تعداد پردازش های موازی. پردازش های موازی نمیتواند بیشتر از 255 عدد باشد

در حین پردازش نیز کاربر میتواند روند پردازش شدن به مرور تصویر را ببیند

نکته : در صورت زیاد بودن تعداد پردازش های موازی ممکن است پردازش اخر مجبور به پردازش تعداد پیکسل های خیلی بیشتری شود  بخاطر اینکه الگوریتم تخصیص بخش پردازش سعی میکند تعداد را بر تعداد پردازش ها تقسیم کند و باقیمانده را به پردازش آخر میسپارد

#نکات کلیدی
در صورت که افکت ها با ورودی AreaInfo به درستی روند پیشرفت خود را گزارش نکنند برنامه موفق به شناسایی اتمام پردازش نمی شود

تصویر همیشه به صورت ARGB می باشد و در صورتی که به هر دلیلی مقدار آلفا تعیین نشود آن پیکسل به صورت نامرئی به نمایش در می اید و در صورت فراموش کل تصویر به صورت سفید نمایش داده می شود

افکت ها حتما باید به صورت Public باشند وگرنه توسط برنامه شناسایی نمیشوند