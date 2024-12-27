install 


6

У меня была та же проблема, и я решил ее следующим образом. Сначала обновите Homebrew.

brew update
Второй шаг: Установка конфигурации пакета:

brew install pkg-config
Третий шаг: установите OpenCV, если вы этого еще не сделали

brew install opencv@4
Четвертый шаг: Теперь вам нужно добавить OpenCV в ваш pkg-config.

export PKG_CONFIG_PATH="/usr/local/opt/opencv@4/lib/pkgconfig:$PKG_CONFIG_PATH"

export PKG_CONFIG_PATH="/usr/local/opt/opencv@4/lib/pkgconfig"


brew install opencv@2

opencv_annotation --annotations=positive_samples.txt --images=augmented_positive/