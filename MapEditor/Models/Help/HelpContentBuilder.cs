using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Models.Help.ContentBuilders
{
    /// <summary>
    /// Класс контента справки.
    /// </summary>
    public static class HelpContentBuilder
    {
        public static List<BaseHelpSection> BuildContent()
        {
            return new List<BaseHelpSection>
            {
                new HelpSection
                {
                    Title = "Общие сведения",
                    Description = "Переключение режимов работы программы осуществляется с помощью " +
                                  "выбора инструмента из окна инструментов, расположенного в верхнем правом углу. " +
                                  "Выбранный инструмент отмечается наложением полупрозрачного серого цвета. " +
                                  "В левом верхнем углу расположено окно объектов для связи, содержащее список доступных для связи объектов."
                },
                new HelpCompositeSection
                {
                    Title = "Инструменты",
                    InnerHelpSections = new List<BaseHelpSection>
                    {
                        new HelpSection
                        {
                            Title = "Курсор",
                            Description = "В режиме «Курсор» можно выделять элементы (точки, области) " +
                                          "и выполнять операции удаления, изменения свойств элемента. " +
                                          "Выделенный элемент отмечается оранжевым цветом. Операции выполняются " +
                                          "с помощью всплывающего контекстного меню, которое вызывается при нажатии правой кнопкой по выбранному элементу. " +
                                          "Удаление выделенных элементов может быть исполнено при нажатии кнопки DELETE"
                        },
                        new HelpSection
                        {
                            Title = "Рука",
                            Description =
                                "В режиме «Рука» можно перетаскивать элементы с помощью зажатой левой кнопки мыши " +
                                "(точки, точки редактируемой области), меняя их координаты."
                        },
                        new HelpSection
                        {
                            Title = "Точка",
                            Description =
                                "В режиме «Точка» пользователь может создавать точки на карте и создавать между ними связи (пути). " +
                                "Создание точки выполняется нажатием левой кнопки мыши по карте. " +
                                "Пользователю также доступно выделение и удаление выбранных точек. " +
                                "Соединение точек происходит через контекстное меню или при выделении точек с зажатой клавишей «SHIFT». " +
                                "Удаление выделенных элементов может быть исполнено при нажатии кнопки DELETE"
                        },
                        new HelpSection
                        {
                            Title = "Область",
                            Description =
                                "В режиме «Область» можно создавать области на карте, создавая точки области. " +
                                "Чтобы завершить разметку области и отправить данные на сервер, необходимо нажать на любую поставленную точку в области.\n" +
                                "ПРИМЕЧАНИЕ: Точки области нельзя двигать в режиме «Область», для перетаскивания точек воспользуйтесь инструментом «Рука»."
                        },
                        new HelpSection
                        {
                            Title = "Маршрут",
                            Description =
                                "В режиме «Маршрут» пользователь может выделять две точки на карте для тестирования построения маршрута. " +
                                "Маршрут строится сразу после выбора второй точки."
                        },


                    }
                },
                new HelpCompositeSection
                {
                    Title = "Настройки",
                    InnerHelpSections = new List<BaseHelpSection>
                    {
                        new HelpCompositeSection
                        {
                            Title = "Сеть",
                            InnerHelpSections = new List<BaseHelpSection>
                            {
                                new HelpSection
                                {
                                    Title = "Ссылка на API",
                                    Description =
                                        "В данное поле ввода вставляется ссылка на API проекта, ссылку можно получить у разработчика. " +
                                        "Флаг использования суффикса api в ссылке определяет, будет ли добавляться к введенной ссылке текст /api или нет."
                                },
                                new HelpSection
                                {
                                    Title = "Добавление и удаление привязываемых объектов",
                                    Description =
                                        "Добавление и удаление групп объектов выполняется соответствующими кнопками внизу раздела. " +
                                        "При добавлении объекта появляется карточка настройки полей объекта. " +
                                        "Поле «Наименование» отвечает за наименование объекта в приложении, " +
                                        "в поле «Относительный URL» необходимо ввести относительный URL, по которому можно получить объект из API, " +
                                        "например, если объект можно получить по ссылке https://127.0.0.1:8000/terminals, то в поле необходимо вписать terminals. " +
                                        "Поле «Ключ поля в API» должно содержать наименование поля из JSON-схемы объекта в API. " +
                                        "Имя поля - имя поля, отображаемое в приложении. " +
                                        "Флаг «Отображать» отвечает за скрытие/отображение поля в приложении. " +
                                        "Флаг «Первичный ключ» устанавливает, является ли поле первичным ключом " +
                                        "в JSON-схеме объекта в API (обычно поле первичного ключа называется id). " +
                                        "Флаг «Индекс» определяет, будет ли поле использоваться в поиске при привязке или нет.\n" +
                                        "ПРИМЕЧАНИЕ: Обязательным для заполнения является поле первичного ключа, остальные поля заполняются для удобства при связывании."

                                }
                            }
                        },
                        new HelpSection
                        {
                            Title = "Вид",
                            Description =
                                "В данном разделе можно менять различные визуальные параметры при работе с картой."
                        }
                    },
                },
                new HelpSection
                {
                    Title = "Связывание объектов API с точками и областями",
                    Description =
                        "Связывание выполняется с помощью выбора соответсвующего пункта всплывающего контекстного меню точки или области. " +
                        "При связывании открывается окно связи объектов, где можно осуществить поиск по объектам, очистить связь или привязать. " +
                        "Для привязки необходимо выбрать нужный объект и нажать кнопку «Привязать»."
                },
                BuildVideoContent()
            };
        }

        private static VideoHelpSection BuildVideoContent()
        {
            return new VideoHelpSection
            {
                Title = "Видео-справка",
                VideoPath = "Resources/HelpVideos/startWork.mp4",
                InnerHelpSections = new List<BaseHelpSection>
                {
                    new VideoHelpSection
                    {
                        Title = "Настроить параметры объектов API",
                        VideoPath = "Resources/HelpVideos/apiSettings.mp4",
                        InnerHelpSections = new List<BaseHelpSection>
                        {
                            new VideoHelpSection
                            {
                                Title = "Ключ поля в API",
                                VideoPath = "Resources/HelpVideos/fieldName.mp4",
                            },
                            new VideoHelpSection
                            {
                                Title = "Имя поля",
                                VideoPath = "Resources/HelpVideos/verboseName.mp4",
                            },
                            new VideoHelpSection
                            {
                                Title = "Отображать",
                                VideoPath = "Resources/HelpVideos/show.mp4",
                            },
                            new VideoHelpSection
                            {
                                Title = "Первичный ключ",
                                VideoPath = "Resources/HelpVideos/primaryKey.mp4",
                            },
                            new VideoHelpSection
                            {
                                Title = "Индекс",
                                VideoPath = "Resources/HelpVideos/index.mp4",
                            },
                            new VideoHelpSection
                            {
                                Title = "Удалить объект",
                                VideoPath = "Resources/HelpVideos/deleteObject.mp4"
                            }
                        }
                    },
                    new VideoHelpSection
                    {
                        Title = "Добавить точку",
                        VideoPath = "Resources/HelpVideos/addPoint.mp4",
                        InnerHelpSections = new List<BaseHelpSection>
                        {
                            new VideoHelpSection
                            {
                                Title = "Удалить точку",
                                VideoPath = "Resources/HelpVideos/deletePoint.mp4"
                            },
                            new VideoHelpSection
                            {
                                Title = "Подвинуть точку",
                                VideoPath = "Resources/HelpVideos/dragPoint.mp4",
                            },
                            new VideoHelpSection
                            {
                                Title = "Связать точки между собой",
                                VideoPath = "Resources/HelpVideos/linkPoints.mp4",
                            },
                            new VideoHelpSection
                            {
                                Title = "Проверить построение маршрута",
                                VideoPath = "Resources/HelpVideos/makeRoute.mp4",
                            },
                            new VideoHelpSection
                            {
                                Title = "Связать точку с объектом API",
                                VideoPath = "Resources/HelpVideos/pointToAPI.mp4",
                            }
                        }
                    },
                    new VideoHelpSection
                    {
                        Title = "Добавить область",
                        VideoPath = "Resources/HelpVideos/addArea.mp4",
                        InnerHelpSections = new List<BaseHelpSection>
                        {
                            new VideoHelpSection
                            {
                                Title = "Подвинуть точку области",
                                VideoPath = "Resources/HelpVideos/dragArea.mp4",
                            },
                            new VideoHelpSection
                            {
                                Title = "Завершить область",
                                VideoPath = "Resources/HelpVideos/stopEditingArea.mp4",
                                InnerHelpSections = new List<BaseHelpSection>
                                {
                                    new VideoHelpSection
                                    {
                                        Title = "Вернуться к редактированию области",
                                        VideoPath = "Resources/HelpVideos/editArea.mp4",
                                    },
                                    new VideoHelpSection
                                    {
                                        Title = "Удалить область",
                                        VideoPath = "Resources/HelpVideos/deleteArea.mp4",
                                    },
                                    new VideoHelpSection
                                    {
                                        Title = "Связать область с объектом API",
                                        VideoPath = "Resources/HelpVideos/areaToAPI.mp4",
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
